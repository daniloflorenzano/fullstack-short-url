using System.Text.Json.Serialization;

namespace ShortUrlApi.ShortUrl;

public record UpdateShortUrlActiveStatusRequest(
    [property: JsonPropertyName("is_active")]
    bool IsActive
);