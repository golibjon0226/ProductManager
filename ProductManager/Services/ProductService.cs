using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductManager.Models;
using ProductManager.Repositories;

namespace ProductManager.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductService(ILogger<ProductService> logger,
                        IProductRepository productRepository,
                        IWebHostEnvironment webHostEnvironment)
    {
        _productRepository = productRepository;
        _webHostEnvironment = webHostEnvironment;
    }


    public async ValueTask<Result<Product>> CreateProduct(Product model, IFormFile productImage)
    {
        if (model is null)
            return new("Product is invalid");
      
        if (productImage != null)
        {
            string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "productImages");
            Directory.CreateDirectory(uploadFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + productImage.FileName;
            string filePath = Path.Combine(uploadFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                productImage.CopyTo(fileStream);
            }

            model.ImageUrl = $"/productImages/{uniqueFileName}";
        }

        try
        {
            var createdProduct = await _productRepository.AddAsync(model);

            if (createdProduct is null)
                return new("Product is not created");

            return new(true) { Data = createdProduct };
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async ValueTask<Result<Product>> DeleteProduct(ulong productId)
    {

        var existingProduct = _productRepository.GetById(productId);

        if (existingProduct is null)
            return new(false) { ErrorMessage = "Product not found." };
        
        if (!String.IsNullOrEmpty(existingProduct.ImageUrl))
        {
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingProduct.ImageUrl!.TrimStart('/'));

            if (File.Exists(oldImagePath))
            {
                try
                {
                    File.Delete(oldImagePath);
                }
                catch (IOException ex)
                {
                    throw new Exception("Failed to delete the product image.", ex);
                }
            }
        }

        try
        {
            var result = await _productRepository.Remove(existingProduct!);

            return new(true) { Data = result };
        }
        catch (Exception e)
        {
            throw new("Couldn't delete Product.", e);
        }
    }


    public async ValueTask<Result<IEnumerable<Product>>> GetAllAsync()
    {
        var products = await _productRepository.GetAll().ToListAsync();

        if (products is null)
            return new(false) { ErrorMessage = "Any Product not found" };

        try
        {
            return new(true) { Data = products };
        }
        catch (Exception e)
        {
            throw new("Couldn't get Products", e);
        }
    }

    public Result<Product> GetByIdAsync(ulong productId)
    {
        if (productId < 1)
            return new("Given id invalid");

        try
        {
            var result = _productRepository.GetById(productId);

            if (result is null)
                return new("Product given with id Not Found");

            return new(true) { Data = result };
        }
        catch (Exception e)
        {
            throw new("Couldn't get Categories", e);
        }
    }

    public async ValueTask<Result<Product>> UpdateProduct(Product product, IFormFile productImage)
    {
        var oldProduct = _productRepository.GetById(product.Id);

        if (!ProductExists(product.Id))
            return new("Product not found");

        if (oldProduct is not null)
        {
            oldProduct.Name = product.Name;
            oldProduct.Description = product.Description;
            oldProduct.Price = product.Price;
        }

        if (productImage != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "productImages");
                Directory.CreateDirectory(uploadFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + productImage.FileName;


                if (!String.IsNullOrEmpty(oldProduct!.ImageUrl))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, oldProduct.ImageUrl.TrimStart('/'));

                if (File.Exists(oldImagePath))
                    {
                    try
                    {
                        File.Delete(oldImagePath);
                    }
                    catch (IOException ex)
                    {
                        throw new Exception("Failed to delete the product image.", ex);
                    }

                }
                }
            
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var fileStream = new FileStream(Path.Combine(uploadFolder, uniqueFileName), FileMode.Create))
                {
                   productImage.CopyTo(fileStream);
                }

            oldProduct.ImageUrl = $"/productImages/{uniqueFileName}";
        }
        
        try
        {
            

            var result = await _productRepository.Update(oldProduct!);

            return new(true) { Data = result };
        }
        catch (Exception e)
        {
            throw new("Couldn't update Product", e);
        }
    }

    private bool ProductExists(ulong id)
    {
        return _productRepository.GetById(id) != null ? true : false;
    }
}