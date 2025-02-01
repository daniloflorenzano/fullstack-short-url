using System.Text.Json.Serialization;

namespace ShortUrlApi.ShortUrl;

public record CreateShortUrlRequest(
    [property: JsonPropertyName("user_id")]
    Guid UserId,

    [property: JsonPropertyName("long_url")]
    string LongUrl,

    [property: JsonPropertyName("url_code")]
    string? UrlCode,

    [property: JsonPropertyName("expires_at")]
    DateTime? ExpiresAt,

    [property: JsonPropertyName("is_active")]
    bool IsActive
);