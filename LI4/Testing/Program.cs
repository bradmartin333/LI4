using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

Testing.ImageCLI ImageCLI = new Testing.ImageCLI();
Image<Rgba32> image = Image.Load<Rgba32>($@"{Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)}\lenna.png");

Rgba32[] pixelArray = new Rgba32[image.Width * image.Height];
image.CopyPixelDataTo(pixelArray);
for (int i = 0; i < pixelArray.Length; i++)
{
    byte val = pixelArray[i].R > 192 ? byte.MaxValue : byte.MinValue;
    pixelArray[i].R = val;
    pixelArray[i].G = val;
    pixelArray[i].B = val;
}
var image2 = Image.LoadPixelData(pixelArray, image.Width, image.Height);

ImageCLI.ConsoleWriteImage(image2);
