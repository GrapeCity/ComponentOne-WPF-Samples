using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.WPF.C1Chart;

namespace SeriesAxis
{
    /// <summary>
    /// Represents series y-axis.
    /// </summary>
    public class YSeriesAxis : SeriesAxis1
    {
        #region .ctor

        /// <summary>
        /// Creates an instance of YSeriesAxis.
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="series"></param>
        /// <param name="position"></param>
        public YSeriesAxis(C1Chart chart, DataSeries series, AxisPosition position)
            : base(chart, series)
        {
            AxisType = AxisType.Y;
            Position = position;

            if ((position & AxisPosition.Far) > 0)
                AnnoAngle = 90;
            else
                AnnoAngle = -90;

            MinorTickHeight = 0;
        }

        #endregion
    }
}
