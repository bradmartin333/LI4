using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Host
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double Scaling = 0.2; ///
        private SKBitmap Bitmap;
        private bool Rendering = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        private string GetRandomDevImage()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo($@"{Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)}/Dev2/");
            FileInfo[] files = directoryInfo.GetFiles().Where(x => new string[] { ".png", ".jpg" }.Contains(x.Extension)).ToArray();
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

        private void SKprocessed_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point controlSpacePosition = e.GetPosition(SKprocessed);
            int x = (int)Math.Floor(controlSpacePosition.X * Bitmap.Width / SKprocessed.ActualWidth);
            int y = (int)Math.Floor(controlSpacePosition.Y * Bitmap.Height / SKprocessed.ActualHeight);
            SKColor c = Bitmap.GetPixel(x, y);
            Grid.Background = new SolidColorBrush(Color.FromArgb(c.Alpha, c.Red, c.Green, c.Blue));
        }
    }
}
