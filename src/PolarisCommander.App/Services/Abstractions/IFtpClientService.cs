namespace PolarisCommander_App.Services.Abstractions;

public interface IFtpClientService
{
    Task ConnectAsync(string host, int port, string userName, string password, CancellationToken cancellationToken = default);
}