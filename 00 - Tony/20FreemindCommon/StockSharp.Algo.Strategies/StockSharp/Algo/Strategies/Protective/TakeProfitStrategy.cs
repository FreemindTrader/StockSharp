﻿using StockSharp.BusinessEntities;
using StockSharp.Messages;
using System;

namespace StockSharp.Algo.Strategies.Protective
{
    /// <summary>Profit protection strategy.</summary>
    public class TakeProfitStrategy : ProtectiveStrategy
    {
        /// <summary>
        /// To create a strategy <see cref="T:StockSharp.Algo.Strategies.Protective.TakeProfitStrategy" />.
        /// </summary>
        /// <param name="trade">Protected position.</param>
        /// <param name="protectiveLevel">The protective level. If the <see cref="P:StockSharp.Messages.Unit.Type" /> type is equal to <see cref="F:StockSharp.Messages.UnitTypes.Limit" />, then the given price is specified. Otherwise, the shift value from the protected trade <paramref name="trade" /> is specified.</param>
        public TakeProfitStrategy( MyTrade trade, Unit protectiveLevel )
          : this( trade.Order.Direction, trade.Trade.Price, trade.Trade.Volume, protectiveLevel )
        {
        }


        /// <summary>
        /// To create a strategy <see cref="T:StockSharp.Algo.Strategies.Protective.TakeProfitStrategy" />.
        /// </summary>
        /// <param name="protectiveSide">Protected position side.</param>
        /// <param name="protectivePrice">Protected position price.</param>
        /// <param name="protectiveVolume">The protected position volume.</param>
        /// <param name="protectiveLevel">The protective level. If the <see cref="P:StockSharp.Messages.Unit.Type" /> type is equal to <see cref="F:StockSharp.Messages.UnitTypes.Limit" />, then the given price is specified. Otherwise, the shift value from <paramref name="protectivePrice" /> is specified.</param>
        public TakeProfitStrategy( Sides protectiveSide, Decimal protectivePrice, Decimal protectiveVolume, Unit protectiveLevel )
          : base( protectiveSide, protectivePrice, protectiveVolume, protectiveLevel, protectiveSide == 0 )
        {
        }
    }
}
