using SkiaSharp;

namespace Projeto {
	class Program {
		static void Main(string[] args) {

			Console.Write("Imagem de entrada: ");
			string e = Console.ReadLine();
			
			Console.Write("Imagem de saída: ");
			string s = Console.ReadLine();

			using (SKBitmap bitmap = SKBitmap.Decode(e)) {
				Console.WriteLine(bitmap.ColorType);

				unsafe {
					byte* ptr = (byte*)bitmap.GetPixels();

					for (int i = 0; i < bitmap.ByteCount; i+=4)
					{
						ptr[i] = 255; // B
						ptr[i+1] = 0; // G
						ptr[i+2] = 0; // R
						ptr[i+3] = 255; // A
					}
				}

				using (FileStream stream = new FileStream(s + "\\teste.png", FileMode.OpenOrCreate, FileAccess.Write)) {
					bitmap.Encode(stream, SKEncodedImageFormat.Png, 100);
				}
			}
		}
	}
}
