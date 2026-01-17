namespace Colegio.Web.Infrastructure.Exceptions;

public sealed class ApiProblemModel
{
    public string? Type { get; init; }
    public string? Title { get; init; }
    public int? Status { get; init; }
    public string? Detail { get; init; }
    public string? Instance { get; init; }
    
    public Dictionary<string, string[]>? Errors { get; init; }
}