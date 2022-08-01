using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Testing
{
    internal class ImageCLI
    {
        private readonly int[] InputColors = { 
            0x000000, 0x000080, 0x008000, 0x008080, 
            0x800000, 0x800080, 0x808000, 0xC0C0C0, 
            0x808080, 0x0000FF, 0x00FF00, 0x00FFFF, 
            0xFF0000, 0xFF00FF, 0xFFFF00, 0xFFFFFF };

        private readonly HexColor[] ColorTable;

        public ImageCLI(Image<Rgba32> image)
        {
            ColorTable = InputColors.Select(x => new HexColor(x)).ToArray();
            ConsoleWriteImage(image);
        }

        private void ConsoleWritePixel(HexColor cValue)
        {
            char[] rList = new char[] { (char)9617, (char)9618, (char)9619, (char)9608 }; // 1/4, 2/4, 3/4, 4/4
            int[] bestHit = new int[] { 0, 0, 4, int.MaxValue }; //ForeColor, BackColor, Symbol, Score

            for (int rChar = rList.Length; rChar > 0; rChar--)
            {
                for (int cFore = 0; cFore < ColorTable.Length; cFore++)
                {
                    for (int cBack = 0; cBack < ColorTable.Length; cBack++)
                    {
                        int R = (ColorTable[cFore].R * rChar + ColorTable[cBack].R * (rList.Length - rChar)) / rList.Length;
                        int G = (ColorTable[cFore].G * rChar + ColorTable[cBack].G * (rList.Length - rChar)) / rList.Length;
                        int B = (ColorTable[cFore].B * rChar + ColorTable[cBack].B * (rList.Length - rChar)) / rList.Length;
                        int iScore = (cValue.R - R) * (cValue.R - R) + (cValue.G - G) * (cValue.G - G) + (cValue.B - B) * (cValue.B - B);
                        if (!(rChar > 1 && rChar < 4 && iScore > 50000)) // rule out too weird combinations
                        {
                            if (iScore < bestHit[3])
                            {
                                bestHit[3] = iScore; //Score
                                bestHit[0] = cFore;  //ForeColor
                                bestHit[1] = cBack;  //BackColor
                                bestHit[2] = rChar;  //Symbol
                            }
                        }
                    }
                }
            }

            Console.ForegroundColor = (ConsoleColor)bestHit[0];
            Console.BackgroundColor = (ConsoleColor)bestHit[1];
            Console.Write(rList[bestHit[2] - 1]);
        }

        private void ConsoleWriteImage(Image<Rgba32> image)
        {
            int sMax = 50;
            decimal percent = Math.Min(decimal.Divide(sMax, image.Width), decimal.Divide(sMax, image.Height));
            Size dSize = new Size((int)(image.Width * percent), (int)(image.Height * percent));
            image.Mutate(x => x.Resize(dSize.Width, dSize.Height));

            image.ProcessPixelRows(accessor =>
            {
                // Color is pixel-agnostic, but it's implicitly convertible to the Rgba32 pixel type
                Rgba32 transparent = Color.Transparent;

                for (int y = 0; y < accessor.Height; y++)
                {
                    Span<Rgba32> pixelRow = accessor.GetRowSpan(y);

                    // pixelRow.Length has the same value as accessor.Width,
                    // but using pixelRow.Length allows the JIT to optimize away bounds checks:
                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        // Get a reference to the pixel at position x
                        ref Rgba32 pixel = ref pixelRow[x];
                        ConsoleWritePixel(new HexColor(pixel));
                        ConsoleWritePixel(new HexColor(pixel));
                    }
                    Console.WriteLine();
                }
                Console.ResetColor();
            });
        }
    }
}
