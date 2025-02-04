namespace ShortUrlApi.Users;

public sealed class UserCreditsModel
{
    public Guid UserId { get; set; }
    public int Credits { get; set; }
}