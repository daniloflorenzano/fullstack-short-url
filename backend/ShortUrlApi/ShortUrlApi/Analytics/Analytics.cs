using System.ComponentModel.DataAnnotations;

namespace ShortUrlApi.Models;

public sealed record Analytics
{
    [Key]
    public int Id { get; set; }
    public string? Country { get; set; }
    public string? Referrer { get; set; }
    public string? IpAddress { get; set; }
    public string? Language { get; set; }
    public DateTime Timestamp { get; set; }
}