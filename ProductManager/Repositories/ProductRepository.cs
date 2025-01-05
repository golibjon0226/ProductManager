namespace ProductManager.Repositories;

using ProductManager.Data;
using ProductManager.Models;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext dbContext) : base(dbContext) { }
}