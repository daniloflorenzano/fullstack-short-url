using Microsoft.EntityFrameworkCore;
using ShortUrlApi.Analytics;
using ShortUrlApi.ShortUrl;

namespace ShortUrlApi;

public class EfUnitOfWork : DbContext
{
    public EfUnitOfWork(DbContextOptions<EfUnitOfWork> options) : base(options)
    {
    }

    public DbSet<ShortUrlModel> ShortUrls { get; set; }
    public DbSet<AnalyticsModel> Analytics { get; set; }
}