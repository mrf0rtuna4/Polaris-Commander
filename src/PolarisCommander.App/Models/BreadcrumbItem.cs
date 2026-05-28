namespace PolarisCommander_App.Models;

public sealed class BreadcrumbItem
{
    public required string DisplayName { get; init; }

    public required string Path { get; init; }
}