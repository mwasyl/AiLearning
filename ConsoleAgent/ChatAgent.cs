using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAgent
{
    internal static class ChatAgent
    {
        public static async Task RunAsync(IServiceProvider sp)
        {
            var client = sp.GetRequiredService<IChatClient>();

            var chatOptions = sp.GetRequiredService<ChatOptions>();

            var history = new List<ChatMessage>
            {
                new (ChatRole.System, "You are a helpful CLI assistant."),
            };

            Console.WriteLine($"Ask me anything (empty = exit).");

            while(true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) break;
                Console.ResetColor();

                history.Add(new ChatMessage(ChatRole.User, input));

                var response = await client.GetResponseAsync(history, chatOptions);

                Console.WriteLine(response.Text);
                history.AddRange(response.Messages);
            }
        }
    }
}
