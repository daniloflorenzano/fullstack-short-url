using Microsoft.EntityFrameworkCore;
using ShortUrlApi.ShortUrl;

namespace ShortUrlApi;

public class EfUnitOfWork : DbContext
{
    public EfUnitOfWork(DbContextOptions<EfUnitOfWork> options) : base(options)
    {
    }

    public DbSet<ShortUrlModel> ShortUrls { get; set; }
    public DbSet<Models.Analytics> Analytics { get; set; }
}