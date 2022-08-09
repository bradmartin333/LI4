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
            // draw image with disposables
            using SKCanvas canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.White);
            using SKImage image = SKImage.FromEncodedData(GetRandomDevImage());
            using SKBitmap bm = SKBitmap.FromImage(image);
            using SKBitmap bms = new SKBitmap((int)SKraw.ActualWidth, (int)SKraw.ActualHeight);
            bm.ScalePixels(bms, SKFilterQuality.High);
            canvas.DrawBitmap(bms, new SKPoint(0, 0));
        }

        private void OnPaintProcessedSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            // the the canvas and properties
            var canvas = e.Surface.Canvas;

            // make sure the canvas is blank
            canvas.Clear(SKColors.White);

            // draw some text
            var paint = new SKPaint
            {
                Color = SKColors.Black,
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                TextAlign = SKTextAlign.Center,
                TextSize = 24
            };
            var coord = new SKPoint(e.Info.Width / 2, (e.Info.Height + paint.TextSize) / 2);
            canvas.DrawText("SkiaSharp", coord, paint);
        }
    }
}
