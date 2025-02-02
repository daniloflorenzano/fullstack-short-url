using System.ComponentModel.DataAnnotations;
using IdGen;

namespace ShortUrlApi.ShortUrl;

public sealed class ShortUrlModel
{
    [Key] public long Id { get; set; }
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "OriginalUrl is required")]
    [MaxLength(2048, ErrorMessage = "OriginalUrl must be 2048 characters or less")]
    public string LongUrl { get; set; } = string.Empty;

    [MaxLength(30, ErrorMessage = "UrlCode must be 30 characters or less")]
    public string UrlCode { get; set; } = string.Empty;
    public List<Analytics.Analytics> Analytics { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public int Visits => Analytics.Count;

    [Obsolete("This constructor is for EF Core only")]
    public ShortUrlModel()
    {
    }

    public ShortUrlModel(Guid userId, string longUrl, string? urlCode, DateTime? expiresAt, bool isActive)
    {
        UserId = userId;
        Id = GenerateUniqueId();
        
        if (!IsValidUrl(longUrl))
            throw new ArgumentException("Invalid URL", nameof(longUrl));
        
        LongUrl = longUrl;

        if (!string.IsNullOrWhiteSpace(urlCode) && !IsValidUrlCode(urlCode))
            throw new ArgumentException("Invalid URL code", nameof(urlCode));
        
        UrlCode = string.IsNullOrWhiteSpace(urlCode)
            ? GenerateShortUrlCode()
            : urlCode;

        CreatedAt = DateTime.UtcNow;
        ExpiresAt = expiresAt;
        IsActive = isActive;
    }
    
    public static bool IsValidUrl(string url) => Uri.TryCreate(url, UriKind.Absolute, out _);

    public static bool IsValidUrlCode(string urlCode) => urlCode.All(char.IsLetterOrDigit);

    private string GenerateShortUrlCode()
    {
        if (Id == 0)
            throw new InvalidOperationException("Id must be set before generating a short URL code");
        
        const string digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        var base62 = new List<char>();
        
        while (Id > 0)
        {
            base62.Insert(0, digits[(int)(Id % 62)]);
            Id /= 62;
        }

        return new string(base62.ToArray());
    }

    private static long GenerateUniqueId()
    {
        const int epoch = 1738457625; // February 1, 2025 9:55:07 PM GMT-03:00
        
        var snowFlakeIdGenerator = new IdGenerator(
            generatorId: 0,
            options: new IdGeneratorOptions(
                timeSource: new DefaultTimeSource(
                    epoch: new DateTime(epoch, DateTimeKind.Utc)
                )
            )
        );

        return snowFlakeIdGenerator.CreateId();
    }
}