using System.Threading;
using System.Threading.Tasks;

namespace CountryCityApi.Services;

public interface IEmailSender
{
    void Send(string title, string text);
    Task SendAsync(string title, string text, CancellationToken cancellationToken = default);
}
