public class EmailService()
{
    public Task EmailFriend(string friendName, string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Emailing {friendName} with {message}");
        Console.ResetColor();
        return Task.CompletedTask;
    }
}