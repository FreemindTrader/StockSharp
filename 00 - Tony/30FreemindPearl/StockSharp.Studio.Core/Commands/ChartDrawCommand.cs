﻿// Decompiled with JetBrains decompiler
// Type: StockSharp.Studio.Core.Commands.ChartDrawCommand
// Assembly: StockSharp.Studio.Core, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2AA452FA-4D77-495D-BC00-1781FF5794A8
// Assembly location: T:\00-StockSharp\Designer\StockSharp.Studio.Core.dll

using StockSharp.Charting;
using System;

namespace StockSharp.Studio.Core.Commands
{
    public class ChartDrawCommand : BaseStudioCommand
    {
        public ChartDrawCommand( IChartDrawData values )
        {
            IChartDrawData chartDrawData = values;
            if ( chartDrawData == null )
                throw new ArgumentNullException( nameof( values ) );
            this.Values = chartDrawData;
        }

        public IChartDrawData Values { get; }
    }
}
