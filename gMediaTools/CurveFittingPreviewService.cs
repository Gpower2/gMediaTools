using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gMediaTools.Services;

namespace gMediaTools
{
    public class CurveFittingPreviewService
    {
        private const int _xAxisPadding = 10;
        private const int _yAxisPadding = 10;

        private const int _numberOfSteps = 50;

        private const int _pointRadius = 5;

        private readonly CurveFittingFactory _curveFittingFactory = new CurveFittingFactory();

        public Image GetPreviewImage(CurveFittingSettings curveSettings, int imgWidth, int imgHeight)
        {
            // Get the Curve Fitting Function
            ICurveFittingService service = _curveFittingFactory.GetCurveFittingService(curveSettings.CurveFittingType);
            var func = service.GetCurveFittingFunction(
                curveSettings.Data.
                    ToDictionary(
                        k => (double)k.Width * k.Height,
                        v => (double)v.Bitrate / (double)(v.Width * v.Height)
                    )
            );

            // Prepare the image for the graph
            Bitmap bmp = new Bitmap(imgWidth, imgHeight);

            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                // Background
                g.FillRectangle(Brushes.White, 0, 0, imgWidth, imgHeight);

                // X Axis
                g.DrawLine(Pens.Black, 0, imgHeight - _xAxisPadding, imgWidth, imgHeight - _xAxisPadding);

                // Y Axis
                g.DrawLine(Pens.Black, _yAxisPadding, 0, _yAxisPadding, imgHeight);

                // Get points data
                var pointsData = curveSettings.Data
                    .ToDictionary(x => (double)x.Width * x.Height, y => (double)y.Bitrate / (y.Width * y.Height));

                // Find minX, maxX with 10% off in order to have better visual graph
                double minX = pointsData.Min(x => x.Key) * 0.9;
                double maxX = pointsData.Max(x => x.Key) * 1.1;

                // Find minY, maxY with 10% off in order to have better visual graph
                double minY = pointsData.Min(y => y.Value) * 0.9;
                double maxY = pointsData.Max(y => y.Value) * 1.1;

                // Get X step for 100 steps
                double xStep = Convert.ToInt32((maxX - minX) / (double)_numberOfSteps);

                // Get Data Values
                Dictionary<double, double> data = new Dictionary<double, double>();
                for (double x = minX; x < maxX; x += xStep)
                {
                    data.Add(x, func(x));
                }

                // Normalize values to pixels to range [a - b]
                int aX = _xAxisPadding;
                int bX = imgWidth - _xAxisPadding;

                int aY = _yAxisPadding;
                int bY = imgHeight - _yAxisPadding;

                // Normalize the data
                pointsData = pointsData.ScaleValues(minX, maxX, minY, maxY, aX, bX, aY, bY)
                    // Inverse Y values for drawing
                    .InverseYValues(imgHeight);

                // Draw the data points
                foreach (var point in pointsData)
                {
                    g.FillEllipse(Brushes.Red, (float)point.Key, (float)point.Value, _pointRadius, _pointRadius);
                }

                // Normalize the data based on previous scale
                data = data.ScaleValues(minX, maxX, minY, maxY, aX, bX, aY, bY)
                    // Inverse Y values for drawing
                    .InverseYValues(imgHeight);

                // Draw the line
                g.DrawCurve(Pens.Blue, 
                    data
                        // Filter out of scope values 
                        .Where(x => x.Key >= _xAxisPadding)
                        // Convert to PointF structure
                        .Select(x => new PointF((float)x.Key, (float)x.Value))
                        .ToArray()
                );
            }

            return bmp;
        }
    }
}
