using dotenv.net;
using RawJsonImplementation.Models;

DotEnv.Load();
var openAIApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

if (openAIApiKey == null)
    throw new InvalidOperationException("OPENAI_API_KEY environment variable is not set.");

List<ChatMessage> messages = [
    new ChatMessage { Role = ChatRole.Assistant, Content = "Hello, what do you want to do today?" }
];

Console.WriteLine(messages[0].Content);

var aiService = new OpenAiService(openAIApiKey);

while(true)
{
    Console.ForegroundColor = ConsoleColor.Blue;
    var input = Console.ReadLine();
    if (input == null || input?.ToLower() == "exit")
        break;

    Console.ResetColor();

    messages.Add(new ChatMessage { Role = ChatRole.User, Content = input! });

    var response = await aiService.CompleteChat(messages);
    messages.Add(response);
    Console.WriteLine(messages.Last().Content);
}
