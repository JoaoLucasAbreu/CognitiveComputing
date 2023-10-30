using SkiaSharp;
using System;
using System.Reflection;

namespace Projeto {
	class Program {

		static void Main(string[] args) {
			using (SKBitmap bitmapEntrada = SKBitmap.Decode("C:\\Users\\joaol\\Documents\\GitHub\\CognitiveComputing\\Erosion\\Floresta foto.jpg"),
				bitmapSaida = new SKBitmap(new SKImageInfo(bitmapEntrada.Width, bitmapEntrada.Height, SKColorType.Gray8))) {

				int tamanhoJanela = 3;
				int metadeJanela = tamanhoJanela / 2;

				unsafe {
					byte* entrada = (byte*)bitmapEntrada.GetPixels();
					byte* saida = (byte*)bitmapSaida.GetPixels();

					for (int y = 0; y < bitmapEntrada.Height; y++)
					{
						for (int x = 0; x < bitmapEntrada.Width; x++)
						{
							int i = (y * bitmapEntrada.Width) + x;
							saida[i] = menorValorRegiao(entrada, bitmapEntrada.Width, bitmapSaida.Height, x, y, metadeJanela);
						}
					}

				}

				using (FileStream stream = new FileStream("C:\\Users\\joaol\\Documents\\GitHub\\CognitiveComputing\\Erosion\\floresta_teste.jpg", FileMode.OpenOrCreate, FileAccess.Write)) {
					bitmapSaida.Encode(stream, SKEncodedImageFormat.Png, 100);
				}
			}
		}

		static unsafe byte menorValorRegiao(byte* entrada, int largura, int altura, int x, int y, int metadeJanela)
		{
			int xInicial = x - metadeJanela;
			int yInicial = y - metadeJanela;
			int xFinal = x + metadeJanela;
			int yFinal = y + metadeJanela;

			if (xInicial < 0)
				xInicial = 0;

			if (yInicial < 0)
				yInicial = 0;

			if (xFinal > largura)
				xFinal = largura - 1;
			
			if (yFinal > altura)
				yFinal = altura - 1;

			byte menor = entrada[(yInicial * largura) + xInicial];		

			for (y = yInicial; y <= yFinal; y++)
			{
				for (x = xInicial; x <= xFinal; x++)
				{
					int i = (y * largura) + x;
					if (entrada[i] < menor)
						menor = entrada[i];
				}
			}

			return menor;
		}
	}
}
