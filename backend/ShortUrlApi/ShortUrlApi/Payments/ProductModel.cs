using System.ComponentModel.DataAnnotations;

namespace ShortUrlApi.Payments;

public sealed class ProductModel
{
    [Key] public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string VendorId { get; set; } = string.Empty;

    [Obsolete("This constructor is for EF Core only")]
    public ProductModel()
    {
    }
}