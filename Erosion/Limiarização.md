# Limiarização de imagens

## Conversão de escala de cinza para preto e branco

O processo em si de conversão para preto e branco é relativamente simples partindo de uma imagem em escala de cinza. Basta encontrar um valor X qualquer, que chamamos de limiar, de modo que os pixels com valor menor do que X passarão a ser pretos, ao passo que todos os valores maiores ou iguais a X passarão a ser brancos.

Apesar de parecer muito simples, essa técnica é muito utilizada para deixar na imagem apenas as informações necessárias para a análise. Assim, é comum chamar os pixels brancos de "objeto" ou "área de interesse", e os pixels pretos de "fundo". Dependendo do cenário, contudo, pode ser necessário inverter a saída, de modo a conseguir manter o conceito de "objeto" branco e "fundo" preto. Por exemplo, um texto preto em uma página de papel branca, provavelmente precisaria ter a saída invertida para que se pudesse manter o conceito de "objeto" branco e "fundo" preto.

O processo de escolher um valor X (limiar) e alterar os pixels da imagem de forma binária (ou para preto, ou para branco) é conhecido como limiarização ou [thresholding](https://en.wikipedia.org/wiki/Thresholding_(image_processing)).

A escolha do limiar, por outro lado, não é necessariamente algo simples, ao passo que o limiar pode ser escolhido para a imagem inteira (limiar global), ou pode-se calcular um limiar específico para cada região da imagem (limiar local).

Além disso, a fórmula utilizada para se chegar ao limiar também varia muito, indo desde uma média aritmética simples, passando pela mediana, moda e chegando até outras fórmulas bem mais complexas. Dessa forma, por questões de simplicidade na disciplina, vamos ficar apenas com a média aritmética simples das imagens em escala de cinza. 😅

### Prática

- Baixe a imagem `Gabarito Correto 2.png`, que é uma imagem RGB, e converta ela para escala de cinza utilizando uma média aritmética simples. Em seguida, calcule a média aritmética da imagem em escala de cinza inteira, e utilize essa média como limiar para gerar uma nova imagem em escala de cinza apenas com pixels pretos e brancos. Não se esqueça de inverter a saída, pois o fundo deve sair preto!

- Baixe a imagem `Gabarito Correto 2.png`, que é uma imagem RGB, e converta ela para escala de cinza utilizando a média ponderada das componentes R, G ou B (luminância relativa). Em seguida, calcule a média aritmética da imagem em escala de cinza inteira, e utilize essa média como limiar para gerar uma nova imagem em escala de cinza apenas com pixels pretos e brancos. Não se esqueça de inverter a saída, pois o fundo deve sair preto! Compare o resultado com o resultado da prática anterior. 😊

- **(Avançado / Opcional)** Baixe a imagem `Gabarito Correto 2.png`, que é uma imagem RGB, e converta ela para escala de cinza utilizando uma média aritmética simples. Em seguida, limiarize a imagem em escala de cinza utilizando um limiar local, calculado para cada pixel considerando uma região quadrada (janela) ao redor desse pixel, cuja largura é 10% da largura da imagem e cuja altura também é 10% da largura da imagem. Cuidados:
	- A janela deve estar centralizada no pixel, ou seja, para uma janela com largura de 101 pixels, a janela deve começar 50 pixels antes do pixel em questão, e deve terminar 50 pixels depois do pixel em questão.
	- Atenção aos pixels próximos das extremidades da imagem. Por exemplo, para uma janela com largura de 101 pixels, os pixels com coordenada X menores do que 50, não poderão utilizar a largura completa da janela.

## Remoção de ruído (erosão)

Depois de converter a imagem para preto e branco, é comum que restem alguns pequenos grupos de pixels brancos isolados ao longo da imagem, provenientes de ruídos/imprecisões tanto na imagem original, como no processo de limiarização.

Assim como o próprio processo de limiarização em si, existem diversas formas de remover esse ruído, sendo que algumas são bastante complexas, como aplicação de filtros no domínio da frequência através da [transformada de Fourier bidimensional](https://en.wikipedia.org/wiki/Fourier_transform), que foge um pouco do escopo da disciplina. 😅

Outras técnicas, por outro lado, apresentam resultados bastante satisfatórios, mesmo com um custo computacional bem menor. Uma dessas técnicas é a erosão de pixels.

O processo da erosão é relativamente fácil de explicar: basta trocar um pixel pelo menor valor dentre os N pixels de sua vizinhança (N é a largura da janela)!

Ou seja, assumindo que 0 seja preto, 1 seja branco, e assumindo a aplicação de uma erosão com uma largura de janela de 3 pixels, a imagem 1 abaixo ficaria como a imagem 2:

```
 Imagem 1      Imagem 2
00000000000   00000000000
00110001000   00000000000
00110000000   00000000000
00000011100   00000000000
00000011100   00000001000
00000011100   00000000000
00000000000   00000000000
```

Em linhas gerais, o algoritmo de erosão é descrito da seguinte forma:

```
imgEntrada = Imagem de entrada, em escala de cinza, já limiarizada
imgSaida = Imagem de saída, inicialmente vazia, com as mesmas dimensões de imgEntrada

largura = largura de imgEntrada
altura = altura de imgEntrada

tamanhoJanela = tamanho da janela utilizada para erosão (de preferência, um valor ímpar, maior ou igual a 3, para simplificar)
metadeJanela = tamanhoJanela / 2 (divisão inteira, onde 7 / 2 = 3)

para y de 0 até altura - 1

	para x de 0 até largura - 1

		imgSaida[y, x] = menor valor da região entre imgEntrada[y - metadeJanela, x - metadeJanela] até imgEntrada[y + metadeJanela, x + metadeJanela], tomando cuidado com os pixels próximos das extremidades da imagem, visto que y e x não podem ser negativos, y não pode ser maior do que altura - 1, e x não pode ser maior do que largura - 1

	fim_para

fim_para
```

### Prática

- Baixe a imagem `Floresta Foto.jpg`, que é uma imagem RGB, e converta ela para escala de cinza utilizando uma média aritmética simples. Em seguida, calcule a média aritmética da imagem em escala de cinza inteira, e utilize essa média como limiar para gerar uma nova imagem em escala de cinza apenas com pixels pretos e brancos, invertendo a saída, e grave essa imagem em um arquivo. Em seguida, aproveite a imagem já limiarizada, aplique uma erosão com uma janela de 9 pixels à imagem e grave essa nova imagem em outro arquivo. Compare os dois arquivos. 😊

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
