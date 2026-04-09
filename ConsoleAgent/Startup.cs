using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.AI;
using GeminiDotnet.Extensions.AI;
using GeminiDotnet;
using Anthropic.SDK;
using ConsoleAgent.Services;

namespace ConsoleAgent;

public static class Startup
{
    public static void ConfigureServices(HostApplicationBuilder builder, string provider, string model)
    {
        builder.Services.AddLogging(logging => logging.AddConsole().SetMinimumLevel(LogLevel.Information));
        builder.Services.AddSingleton<ILoggerFactory>(sp => LoggerFactory.Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Information)));

        builder.Services.AddSingleton<IChatClient>(sp =>
        {
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            var client = provider switch
            {
                 "openai" => new OpenAI.Chat.ChatClient(model, Environment.GetEnvironmentVariable("OPENAI_API_KEY")!).AsIChatClient(),
                 "gemini" => new GeminiClient(new GeminiClientOptions
                 {
                     ApiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY")!,
                     ModelId = model
                 }).AsChatClient(),
                //  "claude" => new AnthropicClient(new APIAuthentication(Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY")!)).Messages,
                _ => throw new ArgumentException($"Uknown provider: {provider}")
            };

            return new ChatClientBuilder(client)
                .UseLogging(loggerFactory)
                .UseFunctionInvocation(loggerFactory, c =>
                {
                    c.IncludeDetailedErrors = true;
                }).Build();
        });

        builder.Services.AddTransient<ChatOptions>(sp => new ChatOptions
        {
            ModelId = model,
            Temperature = 1,
            MaxOutputTokens = 5000,
            Tools = [.. FunctionRegistry.GetTools(sp)]
        });

        builder.Services.AddSingleton<WeatherService>(sp =>
        {
            var apiKey = Environment.GetEnvironmentVariable("WEATHER_API_DOTCOM_KEY")!;
            return new WeatherService(apiKey);
        });

        builder.Services.AddSingleton<WardrobeService>();
    }
}