using Microsoft.Extensions.AI;

namespace KompasKompetencji.Web.Assistant.KnowledgeBase;

public static class SearchQuery
{
    public const int UserMessagesInQuery = 2;

    public static string FromConversation(IEnumerable<ChatMessage> messages) =>
        string.Join("\n", messages
            .Where(m => m.Role == ChatRole.User && !string.IsNullOrWhiteSpace(m.Text))
            .TakeLast(UserMessagesInQuery)
            .Select(m => m.Text));
}
