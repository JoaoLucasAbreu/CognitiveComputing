using SkiaSharp;

namespace Projeto {
	class Program {
		static void Main(string[] args) {
			using (
			SKBitmap bitmapEntrada = SKBitmap.Decode("C:\\Users\\joaol\\Documents\\GitHub\\CognitiveComputing\\GreyImages\\img\\spider_man.png"),
			bitmapSaida = new SKBitmap(new SKImageInfo(bitmapEntrada.Width, bitmapEntrada.Height, SKColorType.Gray8))) {

			Console.WriteLine(bitmapEntrada.ColorType);
			Console.WriteLine(bitmapSaida.ColorType);

			unsafe {
				byte* entrada = (byte*)bitmapEntrada.GetPixels();
				byte* saida = (byte*)bitmapSaida.GetPixels();

				int pixelsTotais = bitmapEntrada.Width * bitmapEntrada.Height;

				// e controla a entrada, e vai de 4 em 4 (cada uma das componentes)
				// s controla a saída, e vai de 1 em 1
				for (int e = 0, s = 0; s < pixelsTotais; e += 4, s++) {
					// Para uma imagem BGRA, entrada[e] é a componente B, entrada[e + 1] é G,
					// entrada[e + 2] é R e entrada[e + 3] é A.
					// Como a saída é em escala de cinza, só existe uma componente: saida[s]
					// Aqui, vamos copiar a componente R da entrada para a saída:
					saida[s] = entrada[e + 1];
				}
			}

			using (FileStream stream = new FileStream("C:\\Users\\joaol\\Documents\\GitHub\\CognitiveComputing\\GreyImages\\img\\spider_man_teste.jpg", FileMode.OpenOrCreate, FileAccess.Write)) {
				bitmapSaida.Encode(stream, SKEncodedImageFormat.Png, 100);
			}
		}
		}
	}
}
