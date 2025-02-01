using System.ComponentModel.DataAnnotations;
using IdGen;
using ShortUrlApi.Models;

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
    public Analytics[] Analytics { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public int Visits => Analytics.Length;

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

    private string GenerateShortUrlCode()
    {
        if (Id == 0)
            throw new InvalidOperationException("Id must be set before generating a short URL code");
        
        var idBase64 = Convert.ToBase64String(BitConverter.GetBytes(Id));
        return idBase64;
    }

    private static long GenerateUniqueId()
    {
        var snowFlakeIdGenerator = new IdGenerator(0);
        return snowFlakeIdGenerator.CreateId();
    }
    
    public static bool IsValidUrl(string url) => Uri.TryCreate(url, UriKind.Absolute, out _);

    public static bool IsValidUrlCode(string urlCode) => urlCode.All(char.IsLetterOrDigit);
}