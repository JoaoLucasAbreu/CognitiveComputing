# Entrada e Saída Básica em C#

## Saída de dados através do console

A saída de dados através do console se dá por meio das chamadas `Console.Write()` e `Console.WriteLine()`. O código a seguir produzirá a saída no quadro mais abaixo.

Código

```csharp
int x = 0;
double y = 1.5;
string z = "Teste";

Console.Write("x vale: ");
Console.WriteLine(x);
Console.Write("y vale: ");
Console.WriteLine(y);
Console.Write("z vale: ");
Console.WriteLine(z);
```

Console

```
x vale: 0
y vale: 1,5
z vale: Teste
```

Repare que `Console.Write()` envia o conteúdo para o console, mas mantém as próximas saídas na mesma linha, ao passo que `Console.WriteLine()` envia o conteúdo para o console e pula para a próxima linha.

## Entrada através do console

Da forma mais simples, a leitura de dados do console em C# se dá por meio de texto (`string`) através da chamada `Console.ReadLine()`, como no exemplo abaixo.

Código

```csharp
Console.Write("Digite o nome: ");
string nome = Console.ReadLine();

Console.Write("O nome é ");
Console.WriteLine(nome);
```

Console

```
Digite o nome: Rafael
O nome é Rafael
```

## Conversão de texto para outros tipos numéricos

Para converter um valor de `string` para um tipo numérico, como `int` ou `double`, basta utilizar os métodos `Parse()` ou `TryParse()` dos respectivos tipos.

```csharp
Console.Write("Digite a idade: ");
string s = Console.ReadLine();
int idade = int.Parse(s);
```

```csharp
Console.Write("Digite o peso: ");
string s = Console.ReadLine();
double peso = double.Parse(s);
```

> Cuidado com `double` e valores decimais! Em computadores com idioma Português, `Console.Write(double)` utiliza `,` como separador decimal, assim como `double.Parse(string)` considera a `,` como separador decimal. Em computadores com idioma Inglês, o `.` é utilizado como separador decimal. Para forçar a utilização de um idioma em específico é necessário utilizar a classe [`CultureInfo`](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo). Para forçar as regras do Inglês, em especial, é possível utilizar [`CultureInfo.InvariantCulture`](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo.invariantculture) como um atalho. 😅

**Importante!** O método `Parse()` produz uma exceção quando o texto não segue o padrão esperado. Por exemplo, `int.Parse("Rafael")` produz uma exceção.

Para evitar essas exceções, é possível utilizar o método `TryParse()`, que se utiliza do operador `out` para devolver o valor convertido, e retorna `false` para indicar falha, ou `true` para indicar sucesso, como no exemplo abaixo:

```csharp
Console.Write("Digite a idade: ");
string s = Console.ReadLine();

int idade;
if (int.TryParse(s, out idade)) {
	Console.Write("Idade fornecida: ");
	Console.WriteLine(idade);
} else {
	Console.WriteLine("Idade inválida!");
}
```

## Prática

- Crie um programa que peça para o usuário fornecer o caminho de uma imagem PNG de entrada e outro caminho de uma imagem PNG de saída. Carregue a imagem PNG de entrada utilizando a classe `SKBitmap` e crie uma nova imagem inteira azul, com as mesmas dimensões da imagem de entrada, gravando essa imagem inteira azul no arquivo PNG de saída.

--------------------------------------------------------------------------------

# Mais referências

CultureInfo
https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo

CultureInfo.InvariantCulture
https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo.invariantculture

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
