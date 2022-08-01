using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO.Compression;

namespace Testing
{
    internal class ImageString
    {
        public static string SendImage(Image<Rgba32> image)
        {
            // Clamp max image dimension
            byte m = 50;
            if (image.Width > m || image.Height > m)
            {
                int r = image.Width / image.Height;
                Size s = r > 1 ? s = new Size(m, m / r) : new Size(m / r, m);
                image.Mutate(x => x.Resize(s.Width, s.Height));
            }

            // Get pixels
            Rgba32[] pixelArray = new Rgba32[image.Width * image.Height];
            image.CopyPixelDataTo(pixelArray);

            // Return compressed image dimension and pixel data array
            byte[] sizeData = new byte[] { (byte)image.Width, (byte)image.Height };
            return Compress(sizeData.Concat(pixelArray.Select(x => x.R)).ToArray());
        }

        private static string Compress(byte[] data)
        {
            MemoryStream output = new();
            using (DeflateStream dstream = new(output, CompressionLevel.Optimal))
                dstream.Write(data, 0, data.Length);
            return Convert.ToBase64String(output.ToArray());
        }

        public static Image<Rgba32> GetImage(string compressed)
        {
            byte[] data = Decompress(compressed);
            Rgba32[] pixelArray = data.Skip(2).Select(x => new Rgba32(x, x, x)).ToArray();
            return Image.LoadPixelData(pixelArray, data[0], data[1]);
        }

        private static byte[] Decompress(string compressed)
        {
            byte[] data = Convert.FromBase64String(compressed);
            MemoryStream input = new(data);
            MemoryStream output = new();
            using (DeflateStream dstream = new(input, CompressionMode.Decompress))
                dstream.CopyTo(output);
            return output.ToArray();
        }
    }
}
