//namespace StockSharp.FxConnectFXCM.Native.Model
//{
//	using System;
//	using System.Reflection;

//	using Ecng.Net;

//	using Newtonsoft.Json;

//	[Obfuscation(Feature = "renaming", ApplyToMembers = true)]
//	[JsonConverter(typeof(JArrayToObjectConverter))]
//	class OrderBookEntry
//	{
//		public decimal Price { get; set; }
//		public decimal Size { get; set; }
//	}

//	[Obfuscation(Feature = "renaming", ApplyToMembers = true)]
//	class OrderBook
//	{
//		[JsonProperty("bids")]
//		public OrderBookEntry[] Bids { get; set; }

//		[JsonProperty("asks")]
//		public OrderBookEntry[] Asks { get; set; }

//		//[JsonProperty("timestamp")]
//		//[JsonConverter(typeof(JsonDateTimeConverter))]
//		//public DateTime Time { get; set; }

//		[JsonProperty("microtimestamp")]
//		[JsonConverter(typeof(JsonDateTimeMcsConverter))]
//		public DateTime Time { get; set; }
//	}
//}