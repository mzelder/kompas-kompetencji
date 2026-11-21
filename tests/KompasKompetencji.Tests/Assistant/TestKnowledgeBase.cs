using KompasKompetencji.Web.Assistant.KnowledgeBase;

namespace KompasKompetencji.Tests.Assistant;

public static class TestKnowledgeBase
{
    private static readonly Lazy<KnowledgeBaseStore> LazyStore = new(() => KnowledgeBaseStore.Load(KnowledgeBaseStore.DefaultPath));
    private static readonly Lazy<Bm25Retriever> LazyRetriever = new(() => new Bm25Retriever(Store));

    public static KnowledgeBaseStore Store => LazyStore.Value;

    public static Bm25Retriever Retriever => LazyRetriever.Value;
}
