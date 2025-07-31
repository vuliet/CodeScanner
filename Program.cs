using System.Text;

Console.OutputEncoding = Encoding.UTF8;

// Bước 1: Quét mã nguồn
var codeSnippets = CodeScanner.ScanCodeFiles(@"D:\MyProject\GoldPriceComparer\GoldPriceComparer");

var gpt4ApiKey = "";

// Bước 2: Tạo embedding cho từng file
var embedder = new EmbeddingHelper(gpt4ApiKey);
var embeddedFiles = new List<(string File, List<float> Embedding)>();

foreach (var code in codeSnippets)
{
    var embedding = await embedder.GetEmbeddingAsync(code);
    var shortPreview = code.Length > 200 ? code.Substring(0, 200) : code;
    embeddedFiles.Add((shortPreview, embedding.ToList()));
}

// Bước 3: Nhập prompt người dùng
var prompt = "Giải thích codebase";

// Bước 4: Tạo embedding cho prompt
var promptEmbedding = await embedder.GetEmbeddingAsync(prompt);

// Bước 5: Truy vấn code liên quan
var results = SemanticSearcher.FindMostRelevantCode(embeddedFiles, [.. promptEmbedding]);
var topContextCode = string.Join("\n---\n", results.Select(r => r.File));

// Bước 6: Gọi GPT-4 để sinh code
var gpt = new GPT4Helper(gpt4ApiKey);
var response = await gpt.AskGPT4Async(prompt, topContextCode);

Console.WriteLine("GPT trả lời:\n" + response);

Console.ReadLine();