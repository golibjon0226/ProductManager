using ProductManager.Models;

namespace ProductManager.Services;

public interface IProductService
{
    public ValueTask<Result<IEnumerable<Product>>> GetAllAsync();
    public Result<Product> GetByIdAsync(ulong productId);
    public ValueTask<Result<Product>> DeleteProduct(ulong productId);
    public ValueTask<Result<Product>> UpdateProduct(Product product, IFormFile producImage);
    public ValueTask<Result<Product>> CreateProduct(Product product, IFormFile productImage);
}