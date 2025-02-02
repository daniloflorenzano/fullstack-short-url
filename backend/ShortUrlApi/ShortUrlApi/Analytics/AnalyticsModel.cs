using System.ComponentModel.DataAnnotations;

namespace ShortUrlApi.Analytics;

public sealed record AnalyticsModel
{
    [Key]
    public int Id { get; set; }
    public string? Referrer { get; set; }
    public string? UserAgent { get; set; }
    public string? IpAddress { get; set; }
    public string? Language { get; set; }
    public DateTime Timestamp { get; set; }
}