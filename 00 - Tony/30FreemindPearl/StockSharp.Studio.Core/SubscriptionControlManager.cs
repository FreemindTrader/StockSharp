﻿// Decompiled with JetBrains decompiler
// Type: StockSharp.Studio.Core.SubscriptionControlManager
// Assembly: StockSharp.Studio.Core, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2AA452FA-4D77-495D-BC00-1781FF5794A8
// Assembly location: T:\00-StockSharp\Designer\StockSharp.Studio.Core.dll

using Ecng.Collections;
using StockSharp.Algo;
using StockSharp.Messages;
using System;

namespace StockSharp.Studio.Core
{
    public class SubscriptionControlManager
    {
        private readonly SynchronizedDictionary<Subscription, SubscriptionControlManager.SubInfo> _subscriptions = new SynchronizedDictionary<Subscription, SubscriptionControlManager.SubInfo>();
        private readonly ISubscriptionProvider _provider;

        public SubscriptionControlManager( ISubscriptionProvider provider )
        {
            this._provider = provider;
            provider.SubscriptionStopped += ( Action<Subscription, Exception> )( ( s, _ ) => this.CheckUnsubscribe( s, false ) );
            provider.SubscriptionStarted += ( Action<Subscription> )( s => this.CheckUnsubscribe( s, false ) );
            provider.SubscriptionFailed += ( Action<Subscription, Exception, bool> )( ( s, _1, _2 ) => this.CheckUnsubscribe( s, false ) );
            provider.SubscriptionOnline += ( Action<Subscription> )( s => this.CheckUnsubscribe( s, false ) );
            provider.SubscriptionReceived += ( Action<Subscription, Message> )( ( s, _ ) => this.CheckUnsubscribe( s, false ) );
        }

        private void CheckUnsubscribe( Subscription sub, bool unsub = false )
        {
            SubscriptionControlManager.SubInfo subInfo = this._subscriptions.TryGetValue<Subscription, SubscriptionControlManager.SubInfo>( sub );
            if ( subInfo == null )
                return;
            if ( unsub )
                subInfo.IsSubscribed = false;
            if ( subInfo.IsSubscribed || !sub.State.IsActive() )
                return;
            this._subscriptions.Remove( sub );
            this._provider.UnSubscribe( sub );
        }

        public void Subscribe( object handler, Subscription subscription )
        {
            this._subscriptions.SafeAdd<Subscription, SubscriptionControlManager.SubInfo>( subscription ).Listeners.Add( handler );
            this._provider.Subscribe( subscription );
        }

        public object[ ] Get( Subscription subscription )
        {
            return this._subscriptions.TryGetValue<Subscription, SubscriptionControlManager.SubInfo>( subscription )?.Listeners.ToArray() ?? Array.Empty<object>();
        }

        public void Unsubscribe( Subscription subscription )
        {
            this.CheckUnsubscribe( subscription, true );
        }

        private class SubInfo
        {
            public bool IsSubscribed { get; set; } = true;

            public CachedSynchronizedSet<object> Listeners { get; } = new CachedSynchronizedSet<object>();
        }
    }
}
