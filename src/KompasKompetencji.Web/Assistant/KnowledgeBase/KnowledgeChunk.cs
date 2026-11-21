namespace KompasKompetencji.Web.Assistant.KnowledgeBase;

public sealed record KnowledgeChunk(string Id, string Source, string Location, string Title, string Text);

public sealed record KnowledgeSearchResult(KnowledgeChunk Chunk, double Score);
