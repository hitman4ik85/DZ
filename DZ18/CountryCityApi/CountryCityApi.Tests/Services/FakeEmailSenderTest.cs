using CountryCityApi.Services;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CountryCityApi.Tests.Services;

public class FakeEmailSenderTest
{
    private const string FilePath = "emails.log";

    [Fact]
    public void Send_EmailSavedToFile()
    {
        // Arrange
        var emailSender = new FakeEmailSender();
        var title = "Test Email";
        var text = "This is a test email.";

        // Act
        emailSender.Send(title, text);

        // Assert
        var logContents = File.ReadAllText(FilePath);
        Assert.Contains(title, logContents);
        Assert.Contains(text, logContents);
    }

    [Fact]
    public async Task SendAsync_EmailSavedToFile()
    {
        // Arrange
        var emailSender = new FakeEmailSender();
        var title = "Async Test Email";
        var text = "This is an async test email.";

        // Act
        await emailSender.SendAsync(title, text, CancellationToken.None);

        // Assert
        var logContents = await File.ReadAllTextAsync(FilePath);
        Assert.Contains(title, logContents);
        Assert.Contains(text, logContents);
    }
}
