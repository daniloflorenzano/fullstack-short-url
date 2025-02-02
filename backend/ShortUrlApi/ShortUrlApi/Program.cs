using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShortUrlApi;
using ShortUrlApi.Analytics;
using ShortUrlApi.ShortUrl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "yourdomain.com",
            ValidAudience = "yourdomain.com",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_super_secret_key"))
        };
    });

// var connectionString = builder.Configuration.GetConnectionString("ShortUrlDb");
//
// if (string.IsNullOrWhiteSpace(connectionString))
//     throw new InvalidOperationException("Connection string is required");

builder.Services.AddDbContext<EfUnitOfWork>(options =>
{
    options.UseInMemoryDatabase("ShortUrlDb");
    //options.UseNpgsql(connectionString);
});

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseExceptionHandler(exceptionHandlerApp
        => exceptionHandlerApp.Run(async context
            => await Results.Problem()
                .ExecuteAsync(context)));
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/v1/shorten", async (CreateShortUrlRequest requestDto, [FromServices] EfUnitOfWork unitOfWork) =>
{
    try
    {
        var shortUrl = new ShortUrlModel(
            requestDto.UserId,
            requestDto.LongUrl,
            requestDto.UrlCode,
            requestDto.ExpiresAt,
            requestDto.IsActive
        );

        var response = new {
            short_url_code = shortUrl.UrlCode,
        };

        var existingShortUrl = await unitOfWork
            .ShortUrls
            .FirstOrDefaultAsync(s => s.LongUrl == requestDto.LongUrl && s.UserId == requestDto.UserId);
        
        
        var result = Results.Created($"/v1/shortUrl/{response.short_url_code}", response);
        
        // TODO: implement Idempotency appropriately with a unique key and cache
        // docs: https://www.milanjovanovic.tech/blog/implementing-idempotent-rest-apis-in-aspnetcore
        if (existingShortUrl != null)
            return result;
        
        await unitOfWork.ShortUrls.AddAsync(shortUrl);
        await unitOfWork.SaveChangesAsync();

        return result;
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return Results.Problem(e.Message);
    }
}).AddEndpointFilter(async (efiContext, next) =>
{
    var shortUrlDtoParam = efiContext.GetArgument<CreateShortUrlRequest>(0);

    var validationError = shortUrlDtoParam switch
    {
        { LongUrl: null } => "'long_url' is required",
        { LongUrl.Length: > 2048 } => "'long_url' must be 2048 characters or less",
        { UrlCode.Length: > 30 } => "'url_code' must be 30 characters or less",
        { UrlCode.Length: > 0 } when !ShortUrlModel.IsValidUrlCode(shortUrlDtoParam.UrlCode) => "'url_code' is not a valid URL code",
        { LongUrl.Length: > 0 } when !ShortUrlModel.IsValidUrl(shortUrlDtoParam.LongUrl) => "'long_url' is not a valid URL",
        _ => null
    };

    if (!string.IsNullOrEmpty(validationError))
    {
        return Results.Problem(
            detail: validationError,
            statusCode: StatusCodes.Status400BadRequest,
            title: "Invalid request",
            type: "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        );
    }
    return await next(efiContext);
});

app.MapGet("/api/v1/shortUrl/{urlCode}", (HttpRequest req, string urlCode, [FromServices] EfUnitOfWork unitOfWork) =>
{
    try
    {
        var shortUrl = unitOfWork.ShortUrls.FirstOrDefault(s => 
            s.UrlCode == urlCode 
            && s.IsActive 
            && (s.ExpiresAt > DateTime.UtcNow || s.ExpiresAt == null)
        );

        if (shortUrl == null)
            return Results.NotFound();
        
        var analytics = new AnalyticsModel
        {
            Timestamp = DateTime.UtcNow,
            UserAgent = req.Headers.UserAgent,
            Referrer = req.Headers.Referer,
            IpAddress = req.HttpContext.Connection.RemoteIpAddress?.ToString(),
            Language = req.Headers.AcceptLanguage,
        };
        
        shortUrl.Analytics.Add(analytics);
        unitOfWork.SaveChanges();

        return Results.Redirect(
            url: shortUrl.LongUrl,
            permanent: false // 302
        );
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return Results.Problem(e.Message);
    }
});

// TODO: Implement GET /api/v1/shortUrl/{userId}/urls
// TODO: Implement PUT /api/v1/shortUrl/{urlCode}/active-status | body: { "user_id: GUID, "is_active": bool }
// TODO: Implement DELETE /api/v1/shortUrl/{urlCode} | body: { "user_id: GUID }      is it ok to be a hard delete?

// TODO: Implement GET /api/v1/shortUrl/{urlCode}/analytics
// TODO: Implement GET /api/v1/shortUrl/{urlCode}/analytics/{date}


app.Run();