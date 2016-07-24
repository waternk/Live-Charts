﻿//The MIT License(MIT)

//copyright(c) 2016 Alberto Rodriguez

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using LiveCharts.Defaults;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;

namespace LiveCharts.SeriesAlgorithms
{
    public class StepLineAlgorithm : SeriesAlgorithm, ICartesianSeries
    {
        public StepLineAlgorithm(ISeriesView view) : base(view)
        {
            SeriesOrientation = SeriesOrientation.Horizontal;
            PreferredSelectionMode = TooltipSelectionMode.SharedXValues;
        }

        public override void Update()
        {
            ChartPoint previous = null;

            var width = ChartFunctions.GetUnitWidth(AxisOrientation.X, Chart, View.ScalesXAt);

            foreach (var chartPoint in View.ActualValues.GetPoints(View))
            {
                var x = ChartFunctions.ToDrawMargin(chartPoint.X, AxisOrientation.X, Chart, View.ScalesXAt);

                chartPoint.View = View.GetPointView(chartPoint.View, chartPoint,
                    View.DataLabels ? View.GetLabelPointFormatter()(chartPoint) : null);

                chartPoint.SeriesView = View;

                var stepView = (IStepPointView) chartPoint.View;

                stepView.Value = ChartFunctions.ToDrawMargin(chartPoint.Y, AxisOrientation.Y, Chart,
                    View.ScalesYAt);
                stepView.From = previous == null ? null : (double?) previous.Y;
                stepView.Width = width;

                chartPoint.ChartLocation = new CorePoint(x, stepView.Value);

                chartPoint.View.DrawOrMove(previous, chartPoint, 0, Chart);

                previous = chartPoint;
            }
        }

        double ICartesianSeries.GetMinX(AxisCore axis)
        {
            return AxisLimits.StretchMin(axis);
        }

        double ICartesianSeries.GetMaxX(AxisCore axis)
        {
            return AxisLimits.UnitRight(axis);
        }

        double ICartesianSeries.GetMinY(AxisCore axis)
        {
            return AxisLimits.SeparatorMin(axis);
        }

        double ICartesianSeries.GetMaxY(AxisCore axis)
        {
            return AxisLimits.SeparatorMax(axis);
        }
    }
}
