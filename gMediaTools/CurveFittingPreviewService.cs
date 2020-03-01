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
        private const int _xAxisPadding = 5;
        private const int _yAxisPadding = 5;

        private const int _pointRadius = 3;

        public Image GetPreviewImage(CurveFittingSettings curveSettings, int imgWidth, int imgHeight)
        {
            ICurveFittingService service = new CurveFittingFactory().GetCurveFittingService(curveSettings.CurveFittingType);
            var func = service.GetCurveFittingFunction(
                curveSettings.Data.
                    ToDictionary(
                        k => (double)k.Width * k.Height,
                        v => (double)v.Bitrate / (double)(v.Width * v.Height)
                    )
            );

            Bitmap bmp = new Bitmap(imgWidth, imgHeight);

            using (var g = Graphics.FromImage(bmp))
            {
                // Background
                g.FillRectangle(Brushes.White, 0, 0, imgWidth, imgHeight);

                // X Axis
                g.DrawLine(Pens.Black, 0, imgHeight - _xAxisPadding, imgWidth, imgHeight - _xAxisPadding);

                // Y Axis
                g.DrawLine(Pens.Black, _yAxisPadding, 0, _yAxisPadding, imgHeight);

                // Get points data
                var pointsData = curveSettings.Data
                    .ToDictionary(x => (double)x.Width * x.Height, y => (double)y.Bitrate / (y.Width * y.Height));

                // Define minX
                int minX = 240 * 240;

                // Define maxX
                int maxX = 3840 * 2160;

                // Get X step for 100 steps
                int xStep = Convert.ToInt32((double)(maxX - minX) / 100.0);

                // Get Data Values
                Dictionary<double, double> data = new Dictionary<double, double>();

                for (int x = minX; x < maxX; x += xStep)
                {
                    data.Add(x, func(x));
                }

                // Find minX, maxX
                double minXd = pointsData.Min(x => x.Key);
                double maxXd = pointsData.Max(x => x.Key);

                // Find minY, maxY
                double minY = pointsData.Min(y => y.Value);
                double maxY = pointsData.Max(y => y.Value);

                // Normalize values to pixels to range [a - b]
                int aX = _xAxisPadding;
                int bX = imgWidth - _xAxisPadding;

                int aY = _yAxisPadding;
                int bY = imgHeight - _yAxisPadding;

                // Normalize the data
                pointsData = pointsData.ToDictionary(
                    x => aX + (((x.Key - minXd) * ((double)bX)) / (maxXd - minXd))
                    , y => (double)imgHeight - (aY + (((y.Value - minY) * ((double)bY)) / (maxY - minY)))
                );

                // Draw the data points
                foreach (var point in pointsData)
                {
                    g.FillEllipse(Brushes.Red, (float)point.Key, (float)point.Value, _pointRadius, _pointRadius);
                }

                // Normalize the data based on previous scale
                data = data.ToDictionary(
                    x => aX + (((x.Key - minXd) * ((double)bX)) / (maxXd - minXd))
                    , y => (double)imgHeight - (aY + (((y.Value - minY) * ((double)bY)) / (maxY - minY)))
                );

                // Draw the line
                g.DrawCurve(Pens.Blue, data.Select(x => new PointF((float)x.Key, (float)x.Value)).ToArray());

            }

            return bmp;
        }
    }
}
