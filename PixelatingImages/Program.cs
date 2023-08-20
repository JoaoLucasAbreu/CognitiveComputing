using System.Reflection.Metadata.Ecma335;
using SkiaSharp;

namespace Projeto {
	class Program {
		static void Main(string[] args) {

			using (SKBitmap bitmap = new SKBitmap(new SKImageInfo(256, 1, SKColorType.Bgra8888))) {

				Console.WriteLine(bitmap.ColorType);
				Console.WriteLine(bitmap.ByteCount);
				Console.WriteLine(bitmap.Bytes);
				Console.WriteLine(bitmap.BytesPerPixel);
				Console.WriteLine(bitmap.Width);

				IntPtr pixels = bitmap.GetPixels();

				unsafe {
					byte* ptr = (byte*)pixels;

					for (int i = 0; i < bitmap.ByteCount; i+=4)
					{
						ptr[i] = 0; // B
						ptr[i+1] = 0; // G
						ptr[i+2] = (byte)((i+2)/4); // R
						ptr[i+3] = 255; // A
					}
				}

				using (FileStream stream = new FileStream("C:\\Users\\joaol\\Documents\\GitHub\\CognitiveComputing\\PixelatingImages\\stripe.png", FileMode.OpenOrCreate, FileAccess.Write)) {
				bitmap.Encode(stream, SKEncodedImageFormat.Png, 100);
				}

			}
		}
	}
}
