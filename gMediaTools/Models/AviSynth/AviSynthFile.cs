using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Models.AviSynth
{
    public sealed class AviSynthFile : IDisposable
    {
        public AviSynthClip Clip { get; }

        public AviSynthFile(AviSynthClip avsClip)
        {
            Clip = avsClip;
        }

        #region "AvsAudioReader"

        public byte[] ReadAudioSamples(long nStart, int nAmount)
        {
            byte[] buffer = new byte[nAmount];
            Clip.ReadAudioSamples(buffer, nStart, nAmount);
            return buffer;
        }

        public long ReadAudioSamples(IntPtr buffer, long nStart, int nAmount)
        {
            Clip.ReadAudioSamples(buffer, nStart, nAmount);
            return (long)nAmount;
        }

        #endregion

        #region "AvsVideoReader"

        public void QuickReadVideoFrame(int frameNumber)
        {
            Clip.ReadVideoFrame(IntPtr.Zero, 0, frameNumber);
        }

        public Bitmap GetVideoFrameBitmap(int frameNumber)
        {
            Bitmap bitmap = new Bitmap(Clip.VideoWidth, Clip.VideoHeight, PixelFormat.Format24bppRgb);
            try
            {
                // TODO: Change Bitmap's PixelFormat to the AvsClip's PixelFormat ???
                Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

                BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
                try
                {
                    IntPtr addr = bitmapdata.Scan0;
                    Clip.ReadVideoFrame(addr, bitmapdata.Stride, frameNumber);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapdata);
                }

                bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
            }
            catch (Exception)
            {
                bitmap.Dispose();
                throw;
            }

            return bitmap;
        }

        #endregion

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                if (Clip != null)
                {
                    Clip.Dispose();
                }

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~AviSynthFile()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
