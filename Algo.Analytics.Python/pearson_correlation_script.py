import clr
import numpy as np  # Assuming use of NumSharp as a NumPy substitute

# Add .NET references
clr.AddReference("System")
clr.AddReference("System.Core")
clr.AddReference("System.Threading")
clr.AddReference("System.Threading.Tasks")
clr.AddReference("StockSharp.Algo")
clr.AddReference("StockSharp.Algo.Analytics")
clr.AddReference("StockSharp.BusinessEntities")
clr.AddReference("StockSharp.Storages")
clr.AddReference("StockSharp.Logging")
clr.AddReference("StockSharp.Messages")
clr.AddReference("NumSharp")  # Adding reference to the NumSharp library

from System import DateTime, TimeSpan
from System.Threading.Tasks import Task
from StockSharp.Algo.Analytics import IAnalyticsScript

# The analytic script, calculating Pearson correlation by specified securities.
class pearson_correlation_script(IAnalyticsScript):
    def Run(
        self,
        logs,
        panel,
        securities,
        from_date,
        to_date,
        storage,
        drive,
        format,
        time_frame,
        cancellation_token
    ):
        if not securities
            logs.AddWarningLog("No instruments.")
            return Task.CompletedTask

        closes = []

        for security in securities:
            # stop calculation if user cancel script execution
            if cancellation_token.IsCancellationRequested:
                break

            # get candle storage
            candle_storage = storage.GetTimeFrameCandleMessageStorage(security, time_frame, drive, format)

            # get closing prices
            prices = [float(c.ClosePrice) for c in candle_storage.Load(from_date, to_date)]

            if len(prices) == 0:
                logs.AddWarningLog("No data for {0}", security)
                return Task.CompletedTask

            closes.append(prices)

        # all arrays must be the same length, so truncate longer ones
        min_length = min(len(arr) for arr in closes)
        closes = [arr[:min_length] for arr in closes]

        # calculating correlation using NumSharp
        np_array = np.array(closes)
        matrix = np.corrcoef(np_array)

        # displaying result into heatmap
        ids = [s.ToStringId() for s in securities]
        panel.DrawHeatmap(ids, ids, matrix.tolist())

        return Task.CompletedTask
