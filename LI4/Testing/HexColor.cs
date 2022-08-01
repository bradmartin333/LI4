using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Testing
{
    internal class HexColor
    {
        public int HexValue { get; set; }
        public Color Color { get; set; }
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public HexColor (int value)
        {
            HexValue = value;
            Initialize();
        }

        public HexColor(Rgba32 value)
        {
            var hex = value.ToHex().Substring(0, 6);
            HexValue = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            Initialize();
        }

        private void Initialize()
        {
            R = (HexValue & 0xff0000) >> 16;
            G = (HexValue & 0xff00) >> 8;
            B = (HexValue & 0xff);
            Color = Color.FromRgb((byte)R, (byte)G, (byte)B);
        }
    }
}
