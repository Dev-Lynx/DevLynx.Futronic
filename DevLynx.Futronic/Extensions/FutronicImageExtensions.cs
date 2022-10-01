using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace DevLynx.Futronic.Extensions
{
    public static class FutronicImageExtensions
    {
        /// <summary>
        /// Get current Fingerprint Image
        /// </summary>
        /// <param name="device">Fingerprint device</param>
        /// <param name="bitmap">Bitmap to write image to</param>
        public static void GetImage(this FingerprintDevice device, WriteableBitmap bitmap)
        {
            Span<byte> frame = device.GetFrame();
            
            int width = device._dim.nWidth;
            int height = device._dim.nHeight;

            try
            {
                bitmap.Lock();

                if (bitmap.Format != PixelFormats.Bgr32)
                    return;

                unsafe
                {
                    byte* buff = (byte*)bitmap.BackBuffer.ToPointer();
                    int stride = bitmap.BackBufferStride;

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            int n = (y * width) + x;
                            int loc = (y * stride) + (x * 4);

                            // Brilliant work by: i-programmer.
                            // https://www.i-programmer.info/programming/wpf-workings/527-writeablebitmap.html?start=2
                            byte b = (byte)~frame[n];

                            buff[loc] = b;
                            buff[loc + 1] = b;
                            buff[loc + 2] = b;
                            buff[loc + 3] = 0xFF;
                        }
                    }
                }

                bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
                bitmap.Unlock();
            }
            catch
            {
            }
        }

        public static void ClearImage(WriteableBitmap bitmap)
        {
            try
            {
                bitmap.Lock();

                if (bitmap.Format != PixelFormats.Bgr32)
                    return;

                unsafe
                {
                    byte* buff = (byte*)bitmap.BackBuffer.ToPointer();
                    int stride = bitmap.BackBufferStride;

                    byte b = 0xFF;
                    int height = bitmap.PixelHeight;
                    int width = bitmap.PixelWidth;

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            int loc = (y * stride) + (x * 4);

                            buff[loc] = b;
                            buff[loc + 1] = b;
                            buff[loc + 2] = b;
                            buff[loc + 3] = 0xFF;
                        }
                    }

                    bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
                    bitmap.Unlock();
                }
            }
            catch
            {
            }
        }
    }
}
