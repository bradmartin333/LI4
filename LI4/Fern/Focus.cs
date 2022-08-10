using System;
using System.Collections.Generic;

namespace Fern
{
    public static class Focus
    {
        private const int TileSize = 5; // N x N pixel tiles

        public static unsafe void AnalyzeTiles(uint* ptr, int wid, int hgt, double fl, double fh)
        {
            for (int row = 0; row < hgt; row += TileSize)
                for (int col = 0; col < wid; col += TileSize)
                {
                    List<double> vals = new List<double>();
                    for (int tileRow = 0; tileRow < TileSize; tileRow++)
                        for (int tileCol = 0; tileCol < TileSize; tileCol++)
                        {
                            uint* tilePtr = ptr + (wid * (row + tileRow)) + col + tileCol;
                            vals.Add(*tilePtr);
                        }

                    byte score = Map(Entropy(vals), fl, fh);
                    for (int tileRow = 0; tileRow < TileSize; tileRow++)
                    {
                        for (int tileCol = 0; tileCol < TileSize; tileCol++)
                        {
                            uint* thisPtr = ptr + (wid * (row + tileRow)) + col + tileCol;
                            *thisPtr = (uint)((255 << 24) | (score << 16) | (score << 8) | score);
                        }
                    }
                }
        }

        private static byte Map(double v, double fL, double fH, double tL = 0, double tH = 255) => (byte)(((v - fL) * (tH - tL) / (fH - fL)) + tL);

        /// <summary>
        /// This is what lies behind MathNet's Entropy calculation
        /// </summary>
        /// <param name="vals">
        /// Data from an entire population
        /// </param>
        /// <returns>
        /// Calculated Entropy
        /// </returns>
        private static double Entropy(List<double> vals)
        {
            Dictionary<double, double> dictionary = new Dictionary<double, double>();
            int num = 0;
            foreach (double num2 in vals)
            {
                if (double.IsNaN(num2))
                    return double.NaN;
                if (dictionary.TryGetValue(num2, out double num3))
                    num3 = (dictionary[num2] = num3 + 1.0);
                else
                    dictionary.Add(num2, 1.0);
                num++;
            }
            double num4 = 0.0;
            foreach (KeyValuePair<double, double> keyValuePair in dictionary)
            {
                double num5 = keyValuePair.Value / num;
                num4 += num5 * Math.Log(num5, 2.0);
            }
            return -num4;
        }
    }
}
