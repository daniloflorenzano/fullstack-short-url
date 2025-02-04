using Microsoft.EntityFrameworkCore;
using ShortUrlApi.Analytics;
using ShortUrlApi.Payments;
using ShortUrlApi.ShortUrl;
using ShortUrlApi.Users;

namespace ShortUrlApi;

public class EfUnitOfWork : DbContext
{
    public EfUnitOfWork(DbContextOptions<EfUnitOfWork> options) : base(options)
    {
    }

    public DbSet<ShortUrlModel> ShortUrls { get; set; }
    public DbSet<AnalyticsModel> Analytics { get; set; }
    public DbSet<UserCreditsModel> UserCredits { get; set; }
    public DbSet<ProductModel> Products { get; set; }
}