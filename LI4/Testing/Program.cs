using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

Testing.ImageCLI ImageCLI = new Testing.ImageCLI();
//Image<Rgba32> image = Image.Load<Rgba32>($@"{Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)}\Dev2\084PX52SJV020ZC.jpg");
Image<Rgba32> image = Image.Load<Rgba32>($@"{Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)}\Generated\1_1_1_1_1_1.png");
ImageCLI.ConsoleWriteImage(image);
