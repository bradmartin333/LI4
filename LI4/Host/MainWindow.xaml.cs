using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace Host
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SKBitmap Bitmap;
        private bool Rendering = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        private string GetRandomDevImage()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}");
            FileInfo[] files = directoryInfo.GetFiles().Where(x => new string[] { ".png", ".jpg" }.Contains(x.Extension.ToLower())).ToArray();
            Random random = new Random();
            FileInfo file = files[random.Next(files.Length)];
            return file.FullName;
        }

        private void SKraw_OnPaint(object sender, SKPaintSurfaceEventArgs e)
        {
            Rendering = true;
            using SKCanvas canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.White);
            using SKImage image = SKImage.FromEncodedData(GetRandomDevImage());
            Bitmap = SKBitmap.FromImage(image);
            canvas.DrawBitmap(Bitmap, e.Info.Rect);
            Rendering = false;
            SKprocessed.InvalidateVisual();
        }

        private void SKprocessed_OnPaint(object sender, SKPaintSurfaceEventArgs e)
        {
            if (Rendering) return;
            using SKCanvas canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.White);
            unsafe
            {
                uint* ptr = (uint*)Bitmap.GetPixels().ToPointer();
                Fern.Focus.AnalyzeTiles(ptr, Bitmap.Width, Bitmap.Height, 3, 5);
            }
            canvas.DrawBitmap(Bitmap, e.Info.Rect);
        }
    }
}
