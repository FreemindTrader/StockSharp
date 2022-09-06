﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fx.Definitions
{
    public interface IClosedTradesCollection : IEnumerable< IClosedTrade >
    {
        /// <summary>
        /// Get number of the offers in the collection
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the offer by its index
        /// </summary>
        /// <param name="index">The index of the offer (0 is first)</param>
        /// <returns></returns>
        IClosedTrade this[ int index ] { get; }
    }
}
