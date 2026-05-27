namespace PolarisCommander_App.Models;

public sealed class FileSystemItem
{
    public required string Name { get; init; }

    public required string FullPath { get; init; }

    public bool IsDirectory { get; init; }

    public string Type => IsDirectory ? "Folder" : "File";
}