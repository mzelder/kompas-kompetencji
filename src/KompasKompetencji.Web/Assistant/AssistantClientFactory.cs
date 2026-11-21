using Anthropic;
using Microsoft.Extensions.AI;

namespace KompasKompetencji.Web.Assistant;

public static class AssistantClientFactory
{
    public static IChatClient Create(IServiceProvider services)
    {
        var section = services.GetRequiredService<IConfiguration>().GetSection("Assistant");
        var apiKey = section["ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
            return new MockChatClient();

        var model = section["Model"] ?? throw new InvalidOperationException("Setting 'Assistant:Model' is missing.");

        return new AnthropicClient { ApiKey = apiKey }
            .AsIChatClient(model)
            .AsBuilder()
            .ConfigureOptions(options =>
            {
                options.Temperature = null;
                options.MaxOutputTokens ??= 16000;
            })
            .Build();
    }
}
