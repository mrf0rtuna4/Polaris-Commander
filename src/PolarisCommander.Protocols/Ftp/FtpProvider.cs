using FluentFTP;
using PolarisCommander.Core.Abstractions;
using PolarisCommander.Core.Models;

namespace PolarisCommander.Protocols.Ftp;

public class FtpProvider : IStorageProvider
{
    private readonly AsyncFtpClient _client;

    public FtpProvider(string host, string user, string password)
    {
        _client = new AsyncFtpClient(host, user, password);
    }

    public string Name => "FTP";

    public bool IsRemote => true;

    public async Task ConnectAsync()
    {
        await _client.Connect();
    }

    public async Task DisconnectAsync()
    {
        await _client.Disconnect();
    }

    public async Task<IReadOnlyList<FileItem>> ListAsync(string path)
    {
        var list = await _client.GetListing(path);

        return list.Select(x => new FileItem
        {
            Name = x.Name,
            FullPath = x.FullName,
            IsDirectory = x.Type == FtpObjectType.Directory,
            Size = x.Size,
            Modified = x.Modified
        }).ToList();
    }
}