using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CountryCityApi.Services;

public class FakeEmailSender : IEmailSender
{
    private const string LogFilePath = "emails.log";

    public void Send(string title, string text)
    {
        LogEmail(title, text);
    }

    public async Task SendAsync(string title, string text, CancellationToken cancellationToken = default)
    {
        await LogEmailAsync(title, text, cancellationToken);
    }

    private void LogEmail(string title, string text)
    {
        var message = $"[{DateTime.Now}] {title}: {text}{Environment.NewLine}";
        File.AppendAllText(LogFilePath, message);
    }

    private async Task LogEmailAsync(string title, string text, CancellationToken cancellationToken)
    {
        var message = $"[{DateTime.Now}] {title}: {text}{Environment.NewLine}";
        await File.AppendAllTextAsync(LogFilePath, message, cancellationToken);
    }
}
