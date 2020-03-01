using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools
{
    public class CurveFittingModel
    {
        public int Width { get; set; }

        public int Height { get; set; }

        private int _Bitrate;
        /// <summary>
        /// In bps
        /// </summary>
        public int Bitrate
        {
            get
            {
                return _Bitrate;
            }
            set
            {
                if (value < 100 * 1000)
                {
                    throw new ArgumentOutOfRangeException(nameof(Bitrate), "The Bitrate can't be less than 100 kbps!");
                }
                _Bitrate = value;
            }
        }

        public CurveFittingModel()
        {
        }

        public CurveFittingModel(int width, int height, int bitrate)
        {
            Width = width;
            Height = height;
            Bitrate = bitrate;
        }

        public override string ToString()
        {
            return $"{Width} X {Height} => {Bitrate / 1000} kbps";
        }
    }
}
