class SemanticSearcher
{
    public static List<(string File, double Similarity)> FindMostRelevantCode(
        List<(string File, List<float> Embedding)> codebase,
        List<float> queryEmbedding,
        int topK = 5)
    {
        return codebase
            .Select(item => (
                item.File,
                Similarity: CosineSimilarity(queryEmbedding, item.Embedding)))
            .OrderByDescending(x => x.Similarity)
            .Take(topK)
            .ToList();
    }

    private static double CosineSimilarity(List<float> v1, List<float> v2)
    {
        double dot = 0.0, mag1 = 0.0, mag2 = 0.0;
        for (int i = 0; i < v1.Count; i++)
        {
            dot += v1[i] * v2[i];
            mag1 += v1[i] * v1[i];
            mag2 += v2[i] * v2[i];
        }
        return dot / (Math.Sqrt(mag1) * Math.Sqrt(mag2));
    }
}
