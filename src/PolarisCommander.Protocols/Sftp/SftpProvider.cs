using PolarisCommander.Core.Abstractions;
using PolarisCommander.Core.Models;
using Renci.SshNet;

namespace PolarisCommander.Protocols.Sftp;

public class SftpProvider : IStorageProvider
{
    private readonly SftpClient _client;

    public SftpProvider(string host, string user, string password)
    {
        _client = new SftpClient(host, user, password);
    }

    public string Name => "SFTP";

    public bool IsRemote => true;

    public async Task ConnectAsync()
    {
        await Task.Run(() => _client.Connect());
    }

    public async Task DisconnectAsync()
    {
        await Task.Run(() => _client.Disconnect());
    }

    public async Task<IReadOnlyList<FileItem>> ListAsync(string path)
    {
        return await Task.Run(() =>
        {
            return _client
                .ListDirectory(path)
                .Select(x => new FileItem
                {
                    Name = x.Name,
                    FullPath = x.FullName,
                    IsDirectory = x.IsDirectory,
                    Size = x.Attributes.Size,
                    Modified = x.LastWriteTime
                })
                .ToList();
        });
    }
}