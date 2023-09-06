using System.Reflection.Metadata.Ecma335;
using SkiaSharp;

namespace Projeto {
	class Program {
		static void Main(string[] args) {

			using (SKBitmap bitmap = new SKBitmap(new SKImageInfo(256, 1, SKColorType.Bgra8888))) {

				IntPtr pixels = bitmap.GetPixels();
				unsafe {
					byte* ptr = (byte*)pixels;

					for (int i = 0; i < bitmap.ByteCount; i+=4)
					{
						ptr[i] = 0; // B
						ptr[i+1] = (byte)(i/4); // G
						ptr[i+2] = 0; // R
						ptr[i+3] = 255; // A
					}
				}

				using (FileStream stream = new FileStream("C:\\Users\\joaol\\Documents\\GitHub\\CognitiveComputing\\PixelatingImages\\img\\stripe.png", FileMode.OpenOrCreate, FileAccess.Write)) {
				bitmap.Encode(stream, SKEncodedImageFormat.Png, 100);
				}

			}

			using (SKBitmap bitmap = new SKBitmap(new SKImageInfo(256, 10, SKColorType.Bgra8888))) {

				IntPtr pixels = bitmap.GetPixels();
				unsafe {
					byte* ptr = (byte*)pixels;

					for (int y = 0; y < bitmap.Height; y++){
						for (int x = 0; x < bitmap.Width; x++){
							int i = ((y * bitmap.Width) + x) * 4;
							ptr[i] = 0; // B
							ptr[i+1] = 0; // G
							ptr[i+2] = (byte)(i/4); // R
							ptr[i+3] = 255; // A
						}
					}
				}

				using (FileStream stream = new FileStream("C:\\Users\\joaol\\Documents\\GitHub\\CognitiveComputing\\PixelatingImages\\img\\rectangle.png", FileMode.OpenOrCreate, FileAccess.Write)) {
				bitmap.Encode(stream, SKEncodedImageFormat.Png, 100);
				}

			}

			using (SKBitmap bitmap = new SKBitmap(new SKImageInfo(50, 10, SKColorType.Bgra8888))) {

				IntPtr pixels = bitmap.GetPixels();				
				unsafe {
					byte* ptr = (byte*)pixels;

					for (int y = 0; y < bitmap.Height; y++){
						for (int x = 0; x < bitmap.Width; x++){
							int i = ((y * bitmap.Width) + x) * 4;
							ptr[i] = (byte)(x * 255 / 49); // B
							ptr[i+1] = 0; // G
							ptr[i+2] = 0; // R
							ptr[i+3] = 255; // A
						}
					}
				}

				using (FileStream stream = new FileStream("C:\\Users\\joaol\\Documents\\GitHub\\CognitiveComputing\\PixelatingImages\\img\\mini_rectangle.png", FileMode.OpenOrCreate, FileAccess.Write)) {
				bitmap.Encode(stream, SKEncodedImageFormat.Png, 100);
				}

			}
		}
	}
}
