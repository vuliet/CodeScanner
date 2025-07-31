using System.Text;

Console.OutputEncoding = Encoding.UTF8;

// Bước 1: Quét mã nguồn
var codeSnippets = CodeScanner.ScanCodeFiles(@"D:\MyProject\FRT\vaccine-family-profile-v2");

var gpt4ApiKey = "";

// Bước 2: Tạo embedding cho từng file
var embedder = new EmbeddingHelper(gpt4ApiKey);
var embeddedFiles = new List<(string File, List<float> Embedding)>();

// Số ký tự tối đa mỗi chunk (tùy model, nên từ 1000–3000)

const int chunkSize = 3000;
const int maxLength = 3000;

foreach (var code in codeSnippets)
{
    var chunks = ChunkString(code, chunkSize);

    foreach (var chunk in chunks)
    {
        try
        {
            var embedding = await embedder.GetEmbeddingAsync(chunk);
            var shortPreview = chunk.Length > maxLength ? chunk.Substring(0, maxLength) : chunk;
            embeddedFiles.Add((shortPreview, embedding.ToList()));
            await Task.Delay(300);
        }
        catch
        {
            continue;
        }

    }
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

// Hàm chia đoạn
static List<string> ChunkString(string str, int maxLength)
{
    var result = new List<string>();
    for (int i = 0; i < str.Length; i += maxLength)
    {
        int length = Math.Min(maxLength, str.Length - i);
        result.Add(str.Substring(i, length));
    }
    return result;
}