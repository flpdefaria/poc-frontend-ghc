Integrando o Azure AI Search Service com .NET — Parte 1

O Azure AI Search Service(https://learn.microsoft.com/en-us/azure/search/search-what-is-azure-search?utm_source=chatgpt.com&wt.mc_id=MVP_407589&tabs=indexing%2Cquickstarts) é uma solução gerenciada da Microsoft que oferece uma plataforma completa para criar experiências de busca inteligentes sobre qualquer tipo de dado, seja estruturado ou não estruturado. Ele permite indexar dados de diversas fontes, oferecendo suporte a consultas de texto livre, filtros, ordenação, facetas e recursos de autocompletar. É amplamente utilizado em aplicações corporativas e em cenários de RAG (https://learn.microsoft.com/pt-br/azure/search/retrieval-augmented-generation-overview?tabs=docs&wt.mc_id=MVP_407589).

Nesta primeira parte, vamos focar especificamente na pesquisa Full-Text Search (https://learn.microsoft.com/en-us/azure/search/search-lucene-query-architecture?wt.mc_id=MVP_407589), o tipo de busca mais comum e fundamental dentro do Azure AI Search. Esse tipo de pesquisa permite que o serviço analise o conteúdo textual dos documentos e retorne resultados com base na relevância dos termos encontrados, e não apenas em correspondências exatas. É o que permite buscar por palavras, frases ou expressões e ainda assim obter respostas precisas, mesmo que o texto original tenha variações, plurais ou sinônimos. Ao longo deste exemplo em .NET, veremos como criar um índice, inserir documentos e executar consultas de texto livre, entendendo na prática como o Azure AI Search processa e classifica os resultados conforme o contexto e a relevância do conteúdo.

Durante a criação do recurso, é necessário escolher uma camada de serviço (tier), que define a capacidade de armazenamento, desempenho e limites de índices e documentos. As principais camadas estão descritas na documentação oficial: Camadas e SKUs do Azure AI Search (https://learn.microsoft.com/en-us/azure/search/search-sku-tier?utm_source=chatgpt.com&wt.mc_id=MVP_407589).

A camada Free é ideal para testes e prototipagem, com restrições de uso. Já a Basic é adequada para projetos de pequeno porte. As camadas Standard (S, S2, S3, etc.) são voltadas para workloads maiores e oferecem alta disponibilidade com múltiplas réplicas e partições. Por fim, as camadas Storage-Optimized (L1 e L2) são recomendadas para cenários de grande volume de dados, onde a prioridade é custo por armazenamento. A escolha do tier depende do volume de dados, da taxa de indexação e da necessidade de resiliência da aplicação.

Outro ponto importante é a autenticação e o controle de acesso. O Azure AI Search permite autenticação via Azure RBAC (Role-Based Access Control) ou API Keys. A autenticação via RBAC usa identidades do Azure Entra ID e é ideal para ambientes corporativos com governança e auditoria centralizada. Mais detalhes estão disponíveis em: Habilitar controle de acesso baseado em função (RBAC) no Azure AI Search (https://learn.microsoft.com/en-us/azure/search/search-security-enable-roles?tabs=config-svc-portal%2Cdisable-keys-portal&wt.mc_id=MVP_407589).

Já a autenticação por API Keys é simples e direta, geralmente usada em ambientes de desenvolvimento ou integrações internas. Cada serviço possui uma Admin Key, utilizada para criar e gerenciar índices. Neste artigo, usaremos o modelo com API Key pela praticidade no desenvolvimento. Veja a documentação completa: Gerenciar autenticação com API Keys no Azure AI Search.

Para iniciar, é necessário criar um grupo de recursos e provisionar o serviço de busca em sua assinatura Azure, conforme o guia oficial: Criar um serviço Azure AI Search no Portal do Azure.

Vamos criar um projeto de console em .NET para interagir com o Azure AI Search:

Vamos criar um projeto de console em .NET para interagir com o Azure AI Search:

```bash
mkdir AzureAiSearchServiceOverview
cd AzureAiSearchServiceOverview
dotnet new sln
dotnet new console -n AzureAiSearchServiceOverview.Console
dotnet sln add AzureAiSearchServiceOverview.Console/AzureAiSearchServiceOverview.Console.csproj
```

Dentro do diretório Models, criaremos o modelo que representará os documentos indexados.

using Azure.Search.Documents.Indexes;

namespace AzureAiSearchServiceOverview.Console.Models;

```csharp
public class Book
{
    [SimpleField(IsKey = true, IsFilterable = true)]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [SearchableField(IsFilterable = true, IsSortable = true)]
    public string Name { get; set; } = string.Empty;
    
    [SearchableField]
    public string Description { get; set; } = string.Empty;
    
    [SearchableField(IsFilterable = true, IsSortable = true)]
    public string Author { get; set; } = string.Empty;
    
    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public int PageCount { get; set; }
    
    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public string Genres { get; set; } = string.Empty; // Comma-separated genres
}
```

O atributo [SearchableField] indica que o campo será analisado para busca de texto livre. Já [SimpleField] define campos não analisados, ideais para valores exatos. As propriedades IsFilterable, IsSortable e IsFacetable determinam se o campo poderá ser usado para filtros, ordenações ou agrupamentos. IsKey indica a chave primária do documento dentro do índice.

Para popular o índice, criaremos uma classe de seed com alguns dados de exemplo:

```csharp
using AzureAiSearchServiceOverview.Console.Models;

namespace AzureAiSearchServiceOverview.Console.Seed;

public static class BookData
{
    public static List<Book> GetSampleBooks() =>
    [
        new Book
        {
            Id = "1",
            Name = "The Lord of the Rings: The Fellowship of the Ring",
            Description =
                "The first part of J.R.R. Tolkien’s epic saga, following Frodo Baggins as he embarks on a perilous journey to destroy the One Ring.",
            Author = "J.R.R. Tolkien",
            PageCount = 423,
            Genres = "Fantasy, Adventure, Fiction"
        },

        new Book
        {
            Id = "2",
            Name = "The Lord of the Rings: The Two Towers",
            Description =
                "The second installment of Tolkien’s masterpiece, where the Fellowship is broken and the quest continues across Middle-earth.",
            Author = "J.R.R. Tolkien",
            PageCount = 352,
            Genres = "Fantasy, Adventure, Fiction"
        },

        new Book
        {
            Id = "3",
            Name = "The Lord of the Rings: The Return of the King",
            Description =
                "The final volume of The Lord of the Rings, concluding the epic struggle between the forces of good and Sauron’s darkness.",
            Author = "J.R.R. Tolkien",
            PageCount = 416,
            Genres = "Fantasy, Adventure, Epic"
        },

        new Book
        {
            Id = "4",
            Name = "The Hobbit",
            Description =
                "Bilbo Baggins is swept into an unexpected adventure with dwarves to reclaim their homeland from the dragon Smaug.",
            Author = "J.R.R. Tolkien",
            PageCount = 310,
            Genres = "Fantasy, Adventure, Fiction"
        },

        new Book
        {
            Id = "5",
            Name = "Dune",
            Description =
                "Frank Herbert’s legendary science fiction novel about politics, religion, and ecology on the desert planet Arrakis.",
            Author = "Frank Herbert",
            PageCount = 688,
            Genres = "Science Fiction, Adventure, Classic"
        },

        new Book
        {
            Id = "6",
            Name = "Neuromancer",
            Description =
                "William Gibson’s cyberpunk classic that introduced the Matrix and redefined science fiction for a digital age.",
            Author = "William Gibson",
            PageCount = 271,
            Genres = "Science Fiction, Cyberpunk, Thriller"
        },

        new Book
        {
            Id = "7",
            Name = "Foundation",
            Description =
                "Isaac Asimov’s visionary tale of the fall and rebirth of a galactic empire through the science of psychohistory.",
            Author = "Isaac Asimov",
            PageCount = 296,
            Genres = "Science Fiction, Classic, Space Opera"
        },

        new Book
        {
            Id = "8",
            Name = "Ender's Game",
            Description =
                "A young boy is trained through battle simulations to lead humanity’s defense against an alien species.",
            Author = "Orson Scott Card",
            PageCount = 324,
            Genres = "Science Fiction, Military, Adventure"
        },

        new Book
        {
            Id = "9",
            Name = "Ready Player One",
            Description =
                "In a dystopian future, Wade Watts hunts for an Easter egg inside the OASIS, a massive virtual reality world.",
            Author = "Ernest Cline",
            PageCount = 374,
            Genres = "Science Fiction, Adventure, Gaming"
        },

        new Book
        {
            Id = "10",
            Name = "The Martian",
            Description =
                "An astronaut stranded on Mars must use his ingenuity and engineering skills to survive until rescue.",
            Author = "Andy Weir",
            PageCount = 369,
            Genres = "Science Fiction, Survival, Adventure"
        }
    ];
}
```

Agora, criamos a classe responsável por gerenciar a criação do índice, inserir dados e executar buscas:

```csharp
using Azure;
using Azure.Identity;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using AzureAiSearchServiceOverview.Console.Models;
using AzureAiSearchServiceOverview.Console.Seed;

namespace AzureAiSearchServiceOverview.Console.Services;

public class SearchService(string endpoint, string apiKey)
{
    private readonly AzureKeyCredential _credential = new(apiKey);
    private const string IndexName = "books";
    
    private SearchIndexClient CreateIndexClient() => new(new Uri(endpoint), _credential);
    private SearchClient CreateSearchClient() => new(new Uri(endpoint), IndexName, _credential);
    
    public async Task InitializeAsync()
    {
        var indexClient = CreateIndexClient();
        var searchClient = CreateSearchClient();
        
        await CreateOrUpdateIndexAsync(indexClient);
        await SeedDataAsync(searchClient);
    }

    private async Task CreateOrUpdateIndexAsync(SearchIndexClient indexClient)
    {
        var fieldBuilder = new FieldBuilder();
        var fields = fieldBuilder.Build(typeof(Book));
        
        var index = new SearchIndex(IndexName)
        {
            Fields = fields,
            Suggesters = { new SearchSuggester("sg", nameof(Book.Name), nameof(Book.Author)) }
        };
        
        await indexClient.CreateOrUpdateIndexAsync(index);
    }
    
    private async Task SeedDataAsync(SearchClient searchClient)
    {
        var docs = BookData.GetSampleBooks();
        var batch = IndexDocumentsBatch.Upload(docs);
        var result = await searchClient.IndexDocumentsAsync(batch);
    }
    
    public async Task<List<Book>> FullTextSearchAsync(
        string searchText, 
        string? filter = null, 
        string? orderBy = null, 
        int size = 5)
    {
        var client = CreateSearchClient();
        var options = new SearchOptions
        {
            QueryType = SearchQueryType.Simple,
            Size = size
        };
        
        if (!string.IsNullOrWhiteSpace(filter))
            options.Filter = filter;
        
        if (!string.IsNullOrWhiteSpace(orderBy))
            options.OrderBy.Add(orderBy);
        
        var results = new List<Book>();
        var response = await client.SearchAsync<Book>(searchText, options);
        
        await foreach (var result in response.Value.GetResultsAsync())
        {
            results.Add(result.Document);
        }
        
        return results;
    }
}
```

Sobre a classe acima, temos:

SearchService(string endpoint, string apiKey)
- Recebe o endpoint e a chave de acesso (API Key) do Azure Search.

CreateIndexClient()
- Cria um cliente para gerenciar índices (criar, atualizar, excluir).

CreateSearchClient()
- Cria um cliente para pesquisar e inserir documentos no índice.

InitializeAsync()
- Roda o processo inicial:
- Cria o índice (CreateOrUpdateIndexAsync()).
- Insere os dados de exemplo (SeedDataAsync())

CreateOrUpdateIndexAsync()
- Define os campos do índice com base no modelo Book.
- Cria ou atualiza o índice no Azure.

SeedDataAsync()
- Pega uma lista de livros de exemplo (BookData.GetSampleBooks()).
- Envia esses documentos para o índice usando IndexDocumentsAsync().

FullTextSearchAsync()
- Faz uma busca de texto livre no índice (como uma pesquisa no Google).
- Permite filtrar, ordenar e definir o número máximo de resultados.
- Retorna uma lista de objetos Book encontrados.

Por fim, o Program.cs que executa as consultas:

```csharp
using AzureAiSearchServiceOverview.Console.Models;
using AzureAiSearchServiceOverview.Console.Services;

var endpoint = "<AZURE-SEARCH-ENDPOINT>";
string key = "<AZURE-SEARCH-KEY>";

var service = new SearchService(endpoint, key);
await service.InitializeAsync();

// query 1: full-text search for keywords
var q1 = await service.FullTextSearchAsync("ring OR desert OR dragon");
Console.WriteLine("\n-- Query 1: Full-text search for ring, desert or dragon --");
Print(q1);

// query 2: filter by Fantasy genre, ordered by PageCount desc
var q2 = await service.FullTextSearchAsync(
    "*", 
    filter: "search.ismatch('Fantasy','Genres')", 
    orderBy: "PageCount desc", size: 10);

Console.WriteLine("\n-- Query 2: Fantasy books ordered by PageCount desc (top 10) --");
Print(q2);


// query 3: search for Arrakis or Middle-earth
var q3 = await service.FullTextSearchAsync("Arrakis OR Middle-earth");
Console.WriteLine("\n-- Query 3: Keyword search for Arrakis OR Middle-earth --");
Print(q3);
return;


static void Print(IEnumerable<Book> books)
{
    foreach (var b in books)
    {
        Console.WriteLine($"- {b.Name} | {b.Author} | {b.PageCount} pages | [genres: {b.Genres}]");
    }
}
```

Sobre o código acima, temos

- Cria o SearchService com endpoint e apiKey.
- Inicializa o serviço:
- Cria/atualiza o índice books.
- Faz seed dos livros de exemplo.
- Executa 3 consultas:
- Full-text: "ring OR desert OR dragon".
- Filtro + ordenação: filtra por “Fantasy” e ordena por PageCount desc (top 10).
- Full-text: "Arrakis OR Middle-earth".
- Imprime os resultados (Name | Author | PageCount | Genres).

Execute a aplicação com:

```bash
dotnet run
````

Temos o resultado:

<imagem1: terminal com os resultados das consultas, mostrando os livros encontrados para cada query>

Após a execução, é possível visualizar o índice e os documentos diretamente no portal do Azure, acessando Indexes → books dentro do recurso do Azure AI Search.

<imagem2: portal do Azure AI search, no menu lateral, mostrando a seção Indexes e o índice "books" criado>
<imagem3: screenshot do portal do Azure mostrando o índice "books" e os documentos indexados>

Esta foi a Parte 1, onde o foco foi a pesquisa de texto livre (Full-Text Search).

Nas próximas partes, exploraremos buscas filtradas, facetas, sugestões automáticas e integração com modelos de linguagem do Azure OpenAI.

Você já pode baixar o projeto por esse link, e não esquece de me seguir no LinkedIn!
Até a próxima, abraços!

---

Usando o Azure Speech Service em um aplicativo console .NET

O Azure Speech Service (https://learn.microsoft.com/en-us/azure/ai-services/speech-service/overview?wt.mc_id=MVP_407589) é um serviço cognitivo gerenciado da Microsoft que reúne capacidades de voz como transcrição em tempo real, tradução de fala e síntese de texto em fala com vozes neurais de alta qualidade. As vozes do serviço produzem uma fala extremamente natural, com entonação, ritmo e pausas que se aproximam bastante do discurso humano. Neste artigo a ideia é criar um aplicativo console em .NET 10 que lê um script em Speech Synthesis Markup Language (SSML) e gera um arquivo MP3 com uma conversa fictícia entre dois desenvolvedores discutindo as novidades do .NET 10.

Antes de ir para o código, vale entender o que é o SSML (https://learn.microsoft.com/en-us/azure/ai-services/speech-service/speech-synthesis-markup?wt.mc_id=MVP_407589). Trata-se de uma linguagem baseada em XML que funciona como uma partitura para o motor de síntese de voz: você não indica apenas o que deve ser dito, mas como deve ser dito. Com ela é possível alternar entre múltiplas vozes dentro do mesmo documento, controlar o ritmo e o tom de cada trecho com o elemento `<prosody>`, inserir pausas precisas com `<break>`, adicionar ênfase a palavras específicas com `<emphasis>` e até incluir uma risada natural com `<laugh/>`. A estrutura do documento SSML (https://learn.microsoft.com/en-us/azure/ai-services/speech-service/speech-synthesis-markup-structure?wt.mc_id=MVP_407589) começa sempre com o elemento raiz `<speak>`, que define a versão e o idioma padrão, e cada trecho de fala precisa estar dentro de um elemento `<voice>` com o nome da voz desejada.

Para detalhes sobre preços, consulte o Azure Speech Service pricing (https://azure.microsoft.com/en-us/pricing/details/speech/?wt.mc_id=MVP_407589).

Para começar, crie um Resource Group no portal Azure para organizar os recursos do projeto.

<imagem1: criando um grupo de recursos no portal Azure>

Com o grupo criado, provisione o recurso do Azure Speech Service. Selecione a categoria "AI + Machine Learning", localize o serviço de fala e preencha os dados básicos como nome, região e tier de preço. Para este exemplo, o tier F0 é suficiente.

<imagem2: criando o recurso Azure Speech no portal Azure>

Após o provisionamento, acesse o recurso e navegue até a seção "Keys and Endpoint" para visualizar as credenciais de acesso. O serviço disponibiliza duas chaves e a região onde o recurso foi criado, ambas necessárias para autenticar as chamadas via SDK.

<imagem3: visualizando as chaves de acesso e a região do serviço no portal Azure>

A aplicação vai ler essas credenciais a partir de variáveis de ambiente, o que é uma prática segura para evitar expor segredos no código-fonte. Configure as variáveis `SPEECH_KEY` e `SPEECH_REGION` no seu ambiente de desenvolvimento com os valores obtidos no portal.

<imagem4: salvando as credenciais do Speech Service como variáveis de ambiente>

Com a infraestrutura pronta, crie um novo diretório e inicialize a aplicação console:

```bash
mkdir AzureSpeech
cd AzureSpeech
dotnet new console
```

Em seguida, adicione o pacote oficial do SDK do Azure Speech Service:

```bash
dotnet add package Microsoft.CognitiveServices.Speech
```

O SDK `Microsoft.CognitiveServices.Speech` expõe toda a API de síntese de voz do serviço e suporta a submissão de documentos SSML diretamente, o que é exatamente o que a aplicação vai usar.

A conversa entre os dois desenvolvedores, Alex e Jamie, está descrita em um arquivo `conversation.xml` na raiz do projeto. Veja um trecho representativo:

```xml
<speak version="1.0" xmlns="http://www.w3.org/2001/10/synthesis" xml:lang="en-US">

  <voice name="en-US-Andrew:DragonHDLatestNeural">
    <prosody rate="medium" pitch="+1st">
      Hey Jamie! Have you had a chance to check out what's new in dot-net 10?
    </prosody>
  </voice>

  <voice name="en-US-Ava:DragonHDLatestNeural">
    <prosody rate="medium">
      Oh, absolutely! And the first thing I'm excited about — it's an L-T-S release.
      Three years of support! That's huge for teams that hate living on the edge.
    </prosody>
  </voice>

  <voice name="en-US-Andrew:DragonHDLatestNeural">
    <prosody rate="medium" pitch="+1st">
      <laugh/>
      Never! Okay, let's talk about my personal favorite — C-sharp 14.
      The new <emphasis level="moderate">field</emphasis> keyword for backed properties
      is a game changer.
      <break time="300ms"/>
      No more writing a private backing field just to add a tiny bit of logic to a setter.
    </prosody>
  </voice>

</speak>
```

O elemento raiz `<speak>` define a versão 1.0 do SSML e o idioma padrão como `en-US`. Cada turno da conversa usa um elemento `<voice>` diferente para alternar entre as duas vozes neurais HD: `en-US-Andrew:DragonHDLatestNeural` para Alex e `en-US-Ava:DragonHDLatestNeural` para Jamie. O elemento `<prosody>` controla o ritmo via atributo `rate` e o tom via `pitch`, onde o valor `+1st` eleva a entonação em um semitom, dando uma personalidade ligeiramente mais animada à voz do Alex. O `<break time="300ms"/>` insere pausas precisas de 300 milissegundos para tornar a conversa mais natural, o `<emphasis level="moderate">` destaca palavras-chave como field e o `<laugh/>` é um dos recursos mais interessantes das vozes DragonHD: ele instrui a síntese a inserir uma risada genuína no áudio gerado. Saiba mais sobre como personalizar voz e som com SSML em Customize voice and sound with SSML (https://learn.microsoft.com/en-us/azure/ai-services/speech-service/speech-synthesis-markup-voice?wt.mc_id=MVP_407589).

O restante da conversa cobre os principais temas do .NET 10. Todos esses temas estão detalhados na documentação oficial disponível em What's new in .NET 10 (https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10/overview?wt.mc_id=MVP_407589).

A classe responsável pela comunicação com o Azure Speech Service fica em `Services/AzureTextToSpeechService.cs`:

```csharp
using Microsoft.CognitiveServices.Speech;

namespace AzureSpeech.Services;

public sealed class AzureTextToSpeechService
{
    private readonly string _speechKey;
    private readonly string _speechRegion;

    public AzureTextToSpeechService()
    {
        _speechKey = Environment.GetEnvironmentVariable("SPEECH_KEY") ?? throw new InvalidOperationException("SPEECH_KEY environment variable not set");
        _speechRegion = Environment.GetEnvironmentVariable("SPEECH_REGION") ?? throw new InvalidOperationException("SPEECH_REGION environment variable not set");
    }

    public async Task<byte[]> GenerateMp3Async(string ssml)
    {
        if (string.IsNullOrWhiteSpace(ssml))
            throw new ArgumentException("SSML cannot be null or empty.", nameof(ssml));

        var speechConfig = SpeechConfig.FromSubscription(_speechKey, _speechRegion);
        speechConfig.SetSpeechSynthesisOutputFormat(
            SpeechSynthesisOutputFormat.Audio24Khz160KBitRateMonoMp3);

        using var synthesizer = new SpeechSynthesizer(speechConfig, audioConfig: null);
        var result = await synthesizer.SpeakSsmlAsync(ssml);

        if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            return result.AudioData;

        var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);

        throw new InvalidOperationException(
            $"Speech synthesis failed. Reason: {cancellation.Reason}. Details: {cancellation.ErrorDetails}");
    }
}
```

`AzureTextToSpeechService()`
- Lê as variáveis de ambiente `SPEECH_KEY` e `SPEECH_REGION` no construtor, lançando uma exceção imediata caso alguma delas esteja ausente.

`GenerateMp3Async()`
- Cria um `SpeechConfig` com as credenciais lidas do ambiente, configura o formato de saída como MP3 a 24kHz e 160kbps, instancia um `SpeechSynthesizer` sem saída de áudio local e envia o documento SSML ao serviço via `SpeakSsmlAsync`. Retorna os bytes do áudio sintetizado em caso de sucesso ou lança uma exceção descritiva com o motivo e os detalhes do erro retornados pelo serviço.

Com a classe de serviço pronta, o ponto de entrada em `Program.cs` fica bem direto ao ponto:

```csharp
using AzureSpeech.Services;

var speechService = new AzureTextToSpeechService();

var ssml = await File.ReadAllTextAsync("conversation.xml");

Console.WriteLine("Generating conversation audio...");

var audioData = await speechService.GenerateMp3Async(ssml);

var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "conversation.mp3");
await File.WriteAllBytesAsync(outputPath, audioData);

Console.WriteLine($"Conversation saved to {outputPath} ({audioData.Length / 1024} KB)");
```

O arquivo `conversation.xml` é lido inteiramente como string, passado para `GenerateMp3Async`, e os bytes retornados são gravados diretamente em `conversation.mp3` no diretório de execução. O projeto também configura o `conversation.xml` com a propriedade `CopyToOutputDirectory: PreserveNewest` no `.csproj`, garantindo que o arquivo esteja disponível junto ao executável após o build sem qualquer etapa manual de cópia.

Para executar a aplicação, basta rodar o comando abaixo no terminal:

```bash
dotnet run
```

O processo pode levar alguns segundos dependendo do tamanho do script SSML e da latência de rede com o serviço. Ao concluir, o arquivo `conversation.mp3` estará disponível no diretório de saída.

<imagem5: arquivo conversation.mp3 gerado no diretório de saída após a execução>

O resultado é um áudio de qualidade notável: duas vozes claramente distintas se revezando de forma natural, entonações diferentes entre Alex e Jamie, pausas bem posicionadas e aquela risada genuína do Alex no meio da conversa sobre C# 14. O Azure Speech Service com SSML e as vozes DragonHD entrega um nível de realismo que transforma um arquivo XML em algo que soa como uma gravação de verdade. Se você quiser explorar ainda mais as possibilidades de pronúncia e controle fonético que o SSML oferece, a documentação de pronunciação está disponível em Pronunciation with SSML (https://learn.microsoft.com/en-us/azure/ai-services/speech-service/speech-synthesis-markup-pronunciation?wt.mc_id=MVP_407589).

Você já pode baixar o projeto por esse link, e não esquece de me seguir no LinkedIn!
Até a próxima, abraços!