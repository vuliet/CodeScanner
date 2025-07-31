using OpenAI_API;

class EmbeddingHelper
{
    private readonly OpenAIAPI _api;

    public EmbeddingHelper(string apiKey)
    {
        _api = new OpenAIAPI(apiKey);
    }

    public async Task<float[]> GetEmbeddingAsync(string code)
    {
        var result = await _api.Embeddings.CreateEmbeddingAsync(code);
        return result.Data[0].Embedding;
    }
}
