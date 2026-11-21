using KompasKompetencji.Web.Text;

namespace KompasKompetencji.Web.Assistant.KnowledgeBase;

public sealed class Bm25Retriever
{
    private const double K1 = 1.2;
    private const double B = 0.75;

    private readonly IReadOnlyList<KnowledgeChunk> chunks;
    private readonly Dictionary<string, int>[] termFrequencies;
    private readonly int[] lengths;
    private readonly double averageLength;
    private readonly Dictionary<string, double> inverseDocumentFrequency;

    public Bm25Retriever(KnowledgeBaseStore store)
    {
        chunks = store.Chunks;
        termFrequencies = new Dictionary<string, int>[chunks.Count];
        lengths = new int[chunks.Count];
        var documentFrequency = new Dictionary<string, int>();

        for (var i = 0; i < chunks.Count; i++)
        {
            var terms = SearchTokenizer.Tokenize($"{chunks[i].Title}\n{chunks[i].Text}");
            lengths[i] = terms.Count;
            termFrequencies[i] = terms.GroupBy(t => t).ToDictionary(g => g.Key, g => g.Count());
            foreach (var term in termFrequencies[i].Keys)
                documentFrequency[term] = documentFrequency.GetValueOrDefault(term) + 1;
        }

        averageLength = lengths.Length == 0 ? 0 : lengths.Average();
        inverseDocumentFrequency = documentFrequency.ToDictionary(
            p => p.Key,
            p => Math.Log(1 + (chunks.Count - p.Value + 0.5) / (p.Value + 0.5)));
    }

    public IReadOnlyList<KnowledgeSearchResult> Search(string query, int count = 5)
    {
        var normalized = TextNormalizer.Normalize(query);
        var terms = SearchTokenizer.Words(normalized)
            .Concat(Synonyms.Expand(normalized))
            .Where(inverseDocumentFrequency.ContainsKey)
            .Distinct()
            .ToList();
        if (terms.Count == 0)
            return [];

        var results = new List<KnowledgeSearchResult>();
        for (var i = 0; i < chunks.Count; i++)
        {
            var score = 0.0;
            var lengthNorm = K1 * (1 - B + B * lengths[i] / averageLength);
            foreach (var term in terms)
            {
                if (!termFrequencies[i].TryGetValue(term, out var frequency))
                    continue;
                score += inverseDocumentFrequency[term] * frequency * (K1 + 1) / (frequency + lengthNorm);
            }
            if (score > 0)
                results.Add(new KnowledgeSearchResult(chunks[i], score));
        }

        return results
            .OrderByDescending(r => r.Score)
            .ThenBy(r => r.Chunk.Id, StringComparer.Ordinal)
            .Take(count)
            .ToList();
    }
}
