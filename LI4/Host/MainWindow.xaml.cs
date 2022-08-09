using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.IO;
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
            DirectoryInfo directoryInfo = new DirectoryInfo($@"{Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)}\Dev2\");
            FileInfo[] files = directoryInfo.GetFiles();
            Random random = new Random();
            FileInfo file = files[random.Next(files.Length)];
            return file.FullName;
        }

        private void OnPaintRawSurface(object sender, SKPaintSurfaceEventArgs e)
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

        private void OnPaintProcessedSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if (Rendering) return;
            using SKCanvas canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.White);
            unsafe
            {
                uint* ptr = (uint*)Bitmap.GetPixels().ToPointer();
                for (int i = 0; i < Bitmap.Width * Bitmap.Height; i++)
                    *ptr++ &= 0xE0E0E0FF;
            }
            canvas.DrawBitmap(Bitmap, e.Info.Rect);
        }
    }
}
