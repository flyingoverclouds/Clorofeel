using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using FluxJpeg.Core;
using FluxJpeg.Core.Encoder;

namespace QuidMind.CloRoFeel.FaceRecorder
{
    public static class Extension
    {
        public static void EncodeJpeg(this WriteableBitmap bitmap, MemoryStream stream, int quality = 90)
        {
            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;
            int bands = 3;
            byte[][,] raster = new byte[bands][,];
            for (int i = 0; i < bands; i++)
            {
                raster[i] = new byte[width, height];
            }
            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    int pixel = bitmap.Pixels[width * row + column];
                    raster[0][column, row] = (byte)(pixel >> 16); //R
                    raster[1][column, row] = (byte)(pixel >> 8);  //G
                    raster[2][column, row] = (byte)pixel;         //B
                }
            }
            ColorModel model = new ColorModel { colorspace = ColorSpace.RGB };
            FluxJpeg.Core.Image img = new FluxJpeg.Core.Image(model, raster);

            JpegEncoder encoder = new JpegEncoder(img, quality, stream);
            encoder.Encode();

            stream.Seek(0, SeekOrigin.Begin);

        }
    }
}
