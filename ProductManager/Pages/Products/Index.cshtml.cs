using ProductManager.Data;
using ProductManager.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductManager.Services;

namespace ProductManager.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _service;
        public List<Product>? ProductList { get; set; }
        public IndexModel(IProductService service)
        {
            _service = service;
        }

        public void OnGet()
        {
            ProductList = _service.GetAllAsync().Result.Data!.ToList();
        }
    }
}
