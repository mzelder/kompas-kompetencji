using System.Text.Json;

namespace KompasKompetencji.Web.Assistant.KnowledgeBase;

public sealed class KnowledgeBaseStore
{
    public static string DefaultPath { get; } = Path.Combine(AppContext.BaseDirectory, "Assistant", "knowledge-base.json");

    private KnowledgeBaseStore(IReadOnlyList<KnowledgeChunk> chunks)
    {
        Chunks = chunks;
    }

    public IReadOnlyList<KnowledgeChunk> Chunks { get; }

    public static KnowledgeBaseStore Load(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(
                $"Knowledge base file not found: {path}. Run 'python knowledge-base/build_knowledge_base.py' and rebuild the project.",
                path);
        }

        using var stream = File.OpenRead(path);
        var file = JsonSerializer.Deserialize<KnowledgeBaseFile>(stream, new JsonSerializerOptions(JsonSerializerDefaults.Web))
            ?? throw new InvalidDataException($"Knowledge base file is empty: {path}.");
        if (file.Chunks is not { Count: > 0 })
            throw new InvalidDataException($"Knowledge base file has no chunks: {path}.");

        return new KnowledgeBaseStore(file.Chunks);
    }

    private sealed record KnowledgeBaseFile(List<KnowledgeChunk>? Chunks);
}
