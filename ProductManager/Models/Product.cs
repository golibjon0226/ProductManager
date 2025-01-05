using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ProductManager.Models;

public class Product
{
    public ulong Id { get; set; }
    [Required]
    [DisplayName("Product Name")]
    public string Name { get; set; }

    public string? Description { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public double Price { get; set; }
    [DisplayName("Image")]
    public string? ImageUrl { get; set; }
}
