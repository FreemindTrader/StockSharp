﻿// Decompiled with JetBrains decompiler
// Type: StockSharp.Studio.Core.Commands.EntityCommand`1
// Assembly: StockSharp.Studio.Core, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2AA452FA-4D77-495D-BC00-1781FF5794A8
// Assembly location: T:\00-StockSharp\Designer\StockSharp.Studio.Core.dll

using StockSharp.Algo;
using System;
using System.Collections.Generic;

namespace StockSharp.Studio.Core.Commands
{
    public class EntityCommand<TEntity> : BaseStudioCommand where TEntity : class
    {
        private IEnumerable<TEntity> _entities;

        public EntityCommand( Subscription subscription, TEntity entity )
        {
            Subscription subscription1 = subscription;
            if ( subscription1 == null )
                throw new ArgumentNullException( nameof( subscription ) );
            this.Subscription = subscription1;
            TEntity entity1 = entity;
            if ( ( object )entity1 == null )
                throw new ArgumentNullException( nameof( entity ) );
            this.Entity = entity1;
        }

        public EntityCommand( Subscription subscription, IEnumerable<TEntity> entities )
        {
            Subscription subscription1 = subscription;
            if ( subscription1 == null )
                throw new ArgumentNullException( nameof( subscription ) );
            this.Subscription = subscription1;
            IEnumerable<TEntity> entities1 = entities;
            if ( entities1 == null )
                throw new ArgumentNullException( nameof( entities ) );
            this._entities = entities1;
        }

        public Subscription Subscription { get; }

        public TEntity Entity { get; }

        public IEnumerable<TEntity> Entities
        {
            get
            {
                if ( this._entities == null && ( object )this.Entity != null )
                    this._entities = ( IEnumerable<TEntity> )new TEntity[1]
                    {
            this.Entity
                    };
                return this._entities;
            }
        }
    }
}
