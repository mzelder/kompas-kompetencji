using System.Runtime.CompilerServices;
using Microsoft.Extensions.AI;

namespace KompasKompetencji.Web.Assistant.Privacy;

public sealed class PseudonymizingChatClient(
    IChatClient innerClient,
    Func<CancellationToken, Task<IReadOnlyList<PersonName>>> loadNames) : DelegatingChatClient(innerClient)
{
    public override async Task<ChatResponse> GetResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default) =>
        await GetStreamingResponseAsync(messages, options, cancellationToken).ToChatResponseAsync(cancellationToken);

    public override async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var pseudonymizer = new Pseudonymizer(await loadNames(cancellationToken));
        var safeMessages = messages.Select(m => Pseudonymize(m, pseudonymizer)).ToList();

        var pending = string.Empty;
        ChatResponseUpdate? last = null;
        await foreach (var update in base.GetStreamingResponseAsync(safeMessages, options, cancellationToken))
        {
            last = update;
            if (!update.Contents.Any(c => c is TextContent))
            {
                yield return update;
                continue;
            }

            pending += string.Concat(update.Contents.OfType<TextContent>().Select(c => c.Text));
            var safe = pseudonymizer.SafeLength(pending);
            var ready = pseudonymizer.Restore(pending[..safe]);
            pending = pending[safe..];

            update.Contents = [.. update.Contents.Where(c => c is not TextContent)];
            if (ready.Length > 0)
                update.Contents.Add(new TextContent(ready));
            if (update.Contents.Count > 0)
                yield return update;
        }

        if (pending.Length > 0)
        {
            yield return new ChatResponseUpdate(ChatRole.Assistant, pseudonymizer.Restore(pending))
            {
                ResponseId = last?.ResponseId,
                MessageId = last?.MessageId,
            };
        }
    }

    private static ChatMessage Pseudonymize(ChatMessage message, Pseudonymizer pseudonymizer)
    {
        if (!message.Contents.Any(c => c is TextContent))
            return message;

        var contents = message.Contents
            .Select(c => c is TextContent text ? new TextContent(pseudonymizer.Pseudonymize(text.Text)) : c)
            .ToList();
        return new ChatMessage(message.Role, contents) { AuthorName = message.AuthorName, MessageId = message.MessageId };
    }
}
