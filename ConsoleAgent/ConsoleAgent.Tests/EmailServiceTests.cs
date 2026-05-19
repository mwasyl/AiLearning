using System.Text;
using Xunit;

public class EmailServiceTests
{
    private readonly EmailService _sut = new();

    [Fact]
    public async Task EmailFriend_ReturnsCompletedTask()
    {
        var result = _sut.EmailFriend("Alice", "Hello!");

        await result;
        Assert.True(result.IsCompletedSuccessfully);
    }

    [Fact]
    public async Task EmailFriend_OutputContainsFriendName()
    {
        var output = new StringBuilder();
        Console.SetOut(new System.IO.StringWriter(output));

        await _sut.EmailFriend("Bob", "Hi there");

        Assert.Contains("Bob", output.ToString());
    }

    [Fact]
    public async Task EmailFriend_OutputContainsMessage()
    {
        var output = new StringBuilder();
        Console.SetOut(new System.IO.StringWriter(output));

        await _sut.EmailFriend("Carol", "Meeting at noon");

        Assert.Contains("Meeting at noon", output.ToString());
    }
}
