namespace StockSharp.Algo.Storages
{
	using System;
    using System.Collections.Generic;
    using Ecng.Collections;
	using Ecng.Common;

	using StockSharp.Algo.Candles.Compression;
	using StockSharp.Messages;

	/// <summary>
	/// Storage processor.
	/// </summary>
	public class StorageProcessor
	{
		private readonly SynchronizedSet<long> _fullyProcessedSubscriptions = new();
		
		/// <summary>
		/// Initializes a new instance of the <see cref="StorageProcessor"/>.
		/// </summary>
		/// <param name="settings">Storage settings.</param>
		/// <param name="candleBuilderProvider">Candle builders provider.</param>
		public StorageProcessor(StorageCoreSettings settings, CandleBuilderProvider candleBuilderProvider)
		{
			Settings = settings ?? throw new ArgumentNullException(nameof(settings));
			CandleBuilderProvider = candleBuilderProvider ?? throw new ArgumentNullException(nameof(candleBuilderProvider));
		}

		/// <summary>
		/// Storage settings.
		/// </summary>
		public StorageCoreSettings Settings { get; }

		/// <summary>
		/// Candle builders provider.
		/// </summary>
		public CandleBuilderProvider CandleBuilderProvider { get; }

		/// <summary>
		/// To reset the state.
		/// </summary>
		public void Reset()
		{
			_fullyProcessedSubscriptions.Clear();
		}

		/// <summary>
		/// Process <see cref="MarketDataMessage"/>.
		/// </summary>
		/// <param name="message">Market-data message (uses as a subscribe/unsubscribe in outgoing case, confirmation event in incoming case).</param>
		/// <param name="newOutMessage">New message event.</param>
		/// <returns>Market-data message (uses as a subscribe/unsubscribe in outgoing case, confirmation event in incoming case).</returns>
		public MarketDataMessage ProcessMarketData(MarketDataMessage message, Action<Message> newOutMessage)
		{
			if (message == null)
				throw new ArgumentNullException(nameof(message));

			if (newOutMessage is null)
				throw new ArgumentNullException(nameof(newOutMessage));

			if (message.From == null && Settings.DaysLoad == TimeSpan.Zero)
				return message;

			if (message.IsSubscribe)
			{
				if (message.SecurityId == default)
					return message;

				var transactionId = message.TransactionId;

				/* -------------------------------------------------------------------------------------------------------------------------------------------
				* 
				*  Tony 06 : If MarketDataMessage has Extension Info, we will pass along the message to adapter to download
				*  
				*			 The following Settings.LoadMessages is so fucked up, it keeps building databars from some of it only snapshot
				*			 My Intention is obviously wanna to download databars from the live server so that the bars can be accurate,
				*			 Unfortunately, I forgot ext.ContainsKey( "FullDownload" )
				*			 
				*			 And in a way, the LoadMessages will try to build databars from whatever shit it stores and so the databars are no
				*			 longer accurate.
				*			 
				*			 I have been tracked this problems for ages until one point I can't take it anymore. So I was stepping pretty much 
				*			 a lot of code and then I finally tracked it down to the following building of databars from some Level1, or whatever
				* 
				* ------------------------------------------------------------------------------------------------------------------------------------------- */

				if ( message.ExtensionInfo != null)
				{
					IDictionary<string, object> ext = message.ExtensionInfo;

					if (ext.ContainsKey("ReloadCandles") || ext.ContainsKey( "FullDownload" ) )
					{
						return message;
					}

				}

				var lastTime = Settings.LoadMessages(CandleBuilderProvider, message, newOutMessage);

				if (message.To != null && lastTime != null && message.To <= lastTime)
				{
					_fullyProcessedSubscriptions.Add(transactionId);
					newOutMessage(new SubscriptionFinishedMessage { OriginalTransactionId = transactionId });

					return null;
				}

				if (lastTime != null)
				{
					if (!(message.DataType2 == DataType.MarketDepth && message.From == null && message.To == null))
					{
						var clone = message.TypedClone();
						clone.From = lastTime;
						message = clone;
						message.ValidateBounds();
					}
				}
			}
			else
			{
				if (_fullyProcessedSubscriptions.Remove(message.OriginalTransactionId))
				{
					newOutMessage(new SubscriptionResponseMessage
					{
						OriginalTransactionId = message.TransactionId,
					});

					return null;
				}
			}

			return message;
		}
	}
}