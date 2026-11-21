using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace KompasKompetencji.Web.Auth;

public sealed class LoginTicketStore
{
    private static readonly TimeSpan Lifetime = TimeSpan.FromSeconds(30);

    private readonly ConcurrentDictionary<string, Entry> tickets = new();

    public string Issue(int userId)
    {
        RemoveExpired();
        var ticket = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        tickets[ticket] = new Entry(userId, DateTimeOffset.UtcNow + Lifetime);
        return ticket;
    }

    public int? Redeem(string ticket)
    {
        if (!tickets.TryRemove(ticket, out var entry))
        {
            return null;
        }

        return entry.ExpiresAt > DateTimeOffset.UtcNow ? entry.UserId : null;
    }

    private void RemoveExpired()
    {
        var now = DateTimeOffset.UtcNow;
        foreach (var (key, entry) in tickets)
        {
            if (entry.ExpiresAt <= now)
            {
                tickets.TryRemove(key, out _);
            }
        }
    }

    private sealed record Entry(int UserId, DateTimeOffset ExpiresAt);
}
