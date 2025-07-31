using OpenAI_API;
using OpenAI_API.Embedding;

class EmbeddingHelper
{
    private readonly OpenAIAPI _api;

    public EmbeddingHelper(string apiKey)
    {
        _api = new OpenAIAPI(apiKey);
    }

    public async Task<float[]> GetEmbeddingAsync(string code)
    {
        var result = await _api.Embeddings.CreateEmbeddingAsync(new EmbeddingRequest { Input = code, Model = "text-embedding-3-small" });
        return result.Data[0].Embedding;
    }
}
