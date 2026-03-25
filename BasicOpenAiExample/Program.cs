using dotenv.net;
using OpenAI.Chat;
using OpenAI.Responses;

Console.WriteLine("OPEN AI EXAMPLE");

DotEnv.Load();
var openAIApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

if (openAIApiKey == null)
    throw new InvalidOperationException("OPENAI_API_KEY environment variable is not set.");

ChatClient chatClient = new (model: "gpt-5-nano", apiKey: openAIApiKey);

List<ChatMessage> messages = [
    new AssistantChatMessage("What do you want to do today?")
];

Console.WriteLine(messages[0].Content[0].Text);

while(true)
{
    Console.ForegroundColor = ConsoleColor.Blue;
    var input = Console.ReadLine();
    Console.ResetColor();
    if (input == null || input?.ToLower() == "exit")
        break;
    
    messages.Add(new UserChatMessage(input));

    ChatCompletion completion = chatClient.CompleteChat(messages);

    var response = completion.Content[0].Text;

    messages.Add(new AssistantChatMessage(response));
    Console.WriteLine(response);
}