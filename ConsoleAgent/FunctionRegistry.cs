using ConsoleAgent.Services;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleAgent;

public static class FunctionRegistry
{
    public static IEnumerable<AITool> GetTools(this IServiceProvider sp)
    {
        var weatherService = sp.GetRequiredService<WeatherService>();

        var getWeatherFn = typeof(WeatherService)
            .GetMethod(nameof(WeatherService.GetWeatherInCity), [typeof(string), typeof(CancellationToken)])!;

        yield return AIFunctionFactory.Create(getWeatherFn, weatherService, new AIFunctionFactoryOptions 
        {
            Name = "get_weather",
            Description = "Gets the current weather in a specified city."
        });

        var wardrobeService = sp.GetRequiredService<WardrobeService>();
        
        var getWardrobeFn = typeof(WardrobeService)
            .GetMethod(nameof(WardrobeService.ListClothing))!;

        yield return AIFunctionFactory.Create(getWardrobeFn, wardrobeService, new AIFunctionFactoryOptions 
        {
            Name = "get_wardrobe",
            Description = "Lists the clothing items in the wardrobe."
        });

    }
}