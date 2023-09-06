# Trabalhando com imagens cinzas

## Conversão de RGB para escala de cinza

Nem todo algoritmo necessita de todas as informações de cores que os olhos humanos enxergam.

Para uma série de algoritmos a diferença de luminosidade, contraste, ou outra carcterística qualquer que veremos a seguir, já basta para um bom funcionamento.

Diferente das imagens coloridas, que possuem componentes R, G, B e A, imagens preto e branco, ou em escala de cinza, possuem apenas uma componente. No nosso caso, essa componente terá o tamanho exato de 1 byte (8 bits), até para simplificar toda nossa vida. 😅 Isso nos deixará com 256 possíveis valores para cada pixel, com 0 (0x00) representando preto, 255 (0xFF) representando branco, e os valores intermediários representando tons de cinza mais escuros ou mais claros.

Imagens em escala de cinza não estão limitadas a terem componentes com 1 byte. Imagens de diagnóstico médico, por exemplo, utilizam 2 bytes (16 bits) ou mais de resolução, fornecendo 65536 possíveis valores para cada pixel, ou mais, como é o caso das imagens com componentes com mais de 16 bits!

Essa questão também está relacionada à intensidade máxima de luz com a qual se deseja trabalhar/analisar. Por exemplo, uma parede branca em uma sala fechada iluminada por lâmpadas LED, apesar de "parecer branca" para nossos olhos, é com certeza "menos branca" do que uma parede branca colocada em uma praça a céu aberto iluminada pelo sol, ao meio dia do verão equatorial! 😅 Nossos olhos se encarregam de controlar (aumentar/diminuir) a quantidade de luz que efetivamente chega à retina, de modo que essas duas paredes brancas pareçam branca para nosso cérebro, mesmo uma sendo muito mais escura do que a outra.

Então, "preto" e "branco", 0 e 255, são valores baseados em referenciais distintos!!! Definir esses referenciais vai além do nosso escopo, e vamos apenas utilizar os valores, sem nos preocuparmos com a quantidade luminosa que eles realmente representam. 💖🙏

Ahhhhhhhhhhhh.... Essa questão também vale para imagens RGB. Existem "vermelhos mais vermelhos" do que outros.... e por aí vai! 😅

Ainda, trabalhar com imagens em escala de cinza requer que o domínio de trabalho seja bem conhecido! Existem diferentes técnicas para converter uma imagem colorida em uma imagem em escala de cinza...

## Isolamento das componentes R, G ou B

A forma mais simples de converter uma imagem colorida para escala de cinza é simplesmente descartar duas das três componentes. Por exemplo, criar uma imagem apenas com R, descartando G e B:

```csharp
using (SKBitmap bitmap = SKBitmap.Decode("Caminho da imagem de entrada"),
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
			saida[s] = entrada[e + 2];
		}
	}

	using (FileStream stream = new FileStream("Caminho da imagem de saída", FileMode.OpenOrCreate, FileAccess.Write)) {
		bitmapSaida.Encode(stream, SKEncodedImageFormat.Png, 100);
	}
}
```

### Prática

- Baixe uma imagem da internet (PNG ou JPEG), carregue essa imagem em um bitmap, crie outros três bitmaps vazios `Gray8` de saída, e exporte cada uma das componentes R, G e B da entrada para um bitmap diferente de saída, **utilizando um único laço de repetição**. 😁

## Média aritmética das componentes R, G ou B

Outra forma simples de converter uma imagem colorida para escala de cinza é fazer uma média aritmética das três componentes R, G e B:

```csharp
saida = (byte)((R + G + B) / 3);
```

## Média ponderada das componentes R, G ou B (Luminância relativa)

Apesar de ser simples, a média aritmética não leva em consideração a luminosidade que é efetivamente percebida por nossos olhos. Existem diversas fórmulas para tentar atingir esse objetivo, cada uma privilegiando um ou outro aspecto da visão humana. A [fórmula também utilizada pela W3C](https://www.w3.org/TR/2008/REC-WCAG20-20081211/#relativeluminancedef) é bastante utilizada mundo a fora, e produz resultados bastante satisfatórios:

```csharp
static byte LuminanciaRelativa(byte r, byte g, byte b) {
	double RsRGB = r / 255.0;
	double GsRGB = g / 255.0;
	double BsRGB = b / 255.0;

	double R, G, B;

	if (RsRGB <= 0.03928) R = RsRGB / 12.92; else R = Math.Pow((RsRGB + 0.055) / 1.055, 2.4);
	if (GsRGB <= 0.03928) G = GsRGB / 12.92; else G = Math.Pow((GsRGB + 0.055) / 1.055, 2.4);
	if (BsRGB <= 0.03928) B = BsRGB / 12.92; else B = Math.Pow((BsRGB + 0.055) / 1.055, 2.4);

	return (byte)Math.Min(255.0, 255.0 * ((0.2126 * R) + (0.7152 * G) + (0.0722 * B)));
}
```

### Prática

- Baixe uma imagem da internet (PNG ou JPEG), carregue essa imagem em um bitmap, crie outros dois bitmaps vazios `Gray8` de saída, e gere uma versão do bitmap de entrada utilizando a média aritmética simples em um bitmap de saída, e outra versão utilizando a luminância relativa em outro bitmap diferente de saída, **utilizando um único laço de repetição**. Compare os resultados! 😁

## Diferentes sistemas de cores

O [sistema de cores RGB](https://en.wikipedia.org/wiki/RGB_color_model) armazena em cada componente uma intensidade luminosa correspondente ao espectro luminoso [capturado por cada uma das diferentes células receptoras do olho humano](https://www.olympus-lifescience.com/en/microscope-resource/primer/lightandcolor/humanvisionintro/).

Apesar de ser possível representar qualquer imagem utilizando o sistema de cores RGB, outros sistemas de cores (ou espaços de cores) carregam outros tipos de informação, que podem ser interessantes/necessários para análisar imagens.

## Sistema de cores HSV

O [sistema de cores HSV](https://en.wikipedia.org/wiki/HSL_and_HSV) armazena em suas componentes informações diferentes, mas bastante úteis para determinados casos.

A componente H armazena a matiz (ou hue) que representa a cor básica visualmente percebida (vermelho, verde, azul, ou uma combinação de duas delas). Fica mais fácil perceber essa componente utilizando um seletor de cores de algum programa de edição de imagens. 😅 Para cores como preto, branco ou cinza, H não tem significado, e normalmente é 0.

A componente S armazena a saturação (ou saturation) que representa a "quantidade efetiva de cor" presente na cor. Por exemplo, tons pastéis mais claros e suaves têm "menos cor" do que tons mais vibrantes, como um magenta ou vermelho puro.

A componente V armazena o brilho (ou value) que representa a sensação visual de mais ou menos emissão luminosa da cor. Cores mais escuras têm V menor, ao passo que cores mais claras têm V maior.

```csharp
static void HSV(byte r, byte g, byte b, out byte h, out byte s, out byte v) {
	// Converte para sRGB (0.0 até 1.0)
	double R = (double)r / 255.0;
	double G = (double)g / 255.0;
	double B = (double)b / 255.0;

	double max = Math.Max(Math.Max(R, G), B);
	double min = Math.Min(Math.Min(R, G), B);

	double H = 0;
	double S = ((max == 0) ? 0 : ((max - min) / max));
	double V = max;

	if (max != min) {
		if (max == R) {
			if (G >= B)
				H = (60.0 * ((G - B) / (max - min))) / 360.0;
			else
				H = ((60.0 * ((G - B) / (max - min))) + 360.0) / 360.0;
		} else if (max == G) {
			H = ((60.0 * ((B - R) / (max - min))) + 120.0) / 360.0;
		} else {
			H = ((60.0 * ((R - G) / (max - min))) + 240.0) / 360.0;
		}
	}

	// Aqui, os valores H, S e V estão normalizados de 0 até 1, como variáveis double.
	// Para nossas finalidades, os valores serão convertidos para bytes inteiros, com valores entre 0 e 255.
	h = (byte)(255.0 * H);
	s = (byte)(255.0 * S);
	v = (byte)(255.0 * V);
}
```

### Prática

- Baixe uma imagem da internet (PNG ou JPEG), carregue essa imagem em um bitmap, crie outros três bitmaps vazios `Gray8` de saída, e exporte cada uma das componentes H, S e V da entrada para um bitmap diferente de saída, **utilizando um único laço de repetição**. 😁

## Sistema de cores Lab (CIELAB ou L* a* b*)

Assim como o sistema de cores HSV, o [sistema de cores CIELAB](https://en.wikipedia.org/wiki/CIELAB_color_space) também armazena em suas componentes informações diferentes de RGB, e diferentes do HSV.

A componente L* armazena a luminosidade de forma bastante similar à forma como os seres humanos efetivamente percebem a luminosidade de uma cor.

A componente a* indica o quanto a cor se aproxima do verde (valores negativos) ou do magenta/vermelho (valores positivos).

A componente b* indica o quanto a cor se aproxima do azul (valores negativos) ou do amarelo (valores positivos).

**Cuidado!** Diferente do HSV, ou do RGB, os valores das componentes L* a* b* não têm um limite determinado, e pode exceder os limites -128 / 127, que são os valores facilmente convertidos para os limites de um byte (de 0 até 255).

```csharp
static void Lab(byte r, byte g, byte b, out double l_, out double a_, out double b_) {
	// https://www.mathworks.com/help/images/ref/rgb2lab.html
	// http://colormine.org/convert/rgb-to-xyz
	// http://colormine.org/convert/rgb-to-lab

	// Converte para sRGB (0.0 até 1.0)
	double R = (double)r / 255.0;
	double G = (double)g / 255.0;
	double B = (double)b / 255.0;

	// Converte de sRGB para RGB linear
	R = ((R <= 0.04045) ? (R / 12.92) : Math.Pow((R + 0.055) / 1.055, 2.4));
	G = ((G <= 0.04045) ? (G / 12.92) : Math.Pow((G + 0.055) / 1.055, 2.4));
	B = ((B <= 0.04045) ? (B / 12.92) : Math.Pow((B + 0.055) / 1.055, 2.4));

	// Converte de RGB linear para CIE 1931 XYZ
	double X = 100.0 * (0.4124 * R + 0.3576 * G + 0.1805 * B);
	double Y = 100.0 * (0.2126 * R + 0.7152 * G + 0.0722 * B);
	double Z = 100.0 * (0.0193 * R + 0.1192 * G + 0.9505 * B);

	// Converte de CIE 1931 XYZ para CIE L*a*b*
	// X, Y, Z se baseiam em uma fonte luminosa "D65" (6500K).
	// https://en.wikipedia.org/wiki/Illuminant_D65
	X = X / 95.047;
	X = ((X > 0.008856) ? Math.Pow(X, 1.0 / 3.0) : (X * 7.787 + 16.0 / 116.0));
	Y = Y / 100.0;
	Y = ((Y > 0.008856) ? Math.Pow(Y, 1.0 / 3.0) : (Y * 7.787 + 16.0 / 116.0));
	Z = Z / 108.883;
	Z = ((Z > 0.008856) ? Math.Pow(Z, 1.0 / 3.0) : (Z * 7.787 + 16.0 / 116.0));

	// L* Luminosidade da cor, de 0 (preto) até 100 (branco).
	// a* Quanto a cor se aproxima do verde (valores negativos) ou do magenta/vermelho (valores positivos). Apesar dos limites não serem absolutos / definidos, os valores costumam ficar entre -100 e 100.
	// b* quanto a cor se aproxima do azul (valores negativos) ou do amarelo (valores positivos). Apesar dos limites não serem absolutos / definidos, os valores costumam ficar entre -100 e 100.
	l_ = 116.0 * Y - 16.0;
	a_ = 500.0 * (X - Y);
	b_ = 200.0 * (Y - Z);
}
```

### Prática

- Baixa a imagem `FundoVerde.png`, carregue essa imagem em um bitmap, crie outros três bitmaps vazios `Gray8` de saída, e exporte cada uma das componentes L*, a* (apenas valores negativos) e b* (apenas valores negativos) da entrada para um bitmap diferente de saída, **utilizando um único laço de repetição**. 😁

--------------------------------------------------------------------------------

# Mais referências

SKBitmap.Decode Method
https://learn.microsoft.com/en-us/dotnet/api/skiasharp.skbitmap.decode

SKBitmap.Encode Method
https://learn.microsoft.com/en-us/dotnet/api/skiasharp.skbitmap.encode

FileStream Class
https://learn.microsoft.com/en-us/dotnet/api/system.io.filestream

using statement - ensure the correct use of disposable objects
https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/using

SKBitmap Class
https://learn.microsoft.com/en-us/dotnet/api/skiasharp.skbitmap

SKImageInfo Struct
https://learn.microsoft.com/en-us/dotnet/api/skiasharp.skimageinfo
