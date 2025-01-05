using ProductManager.Data;
using ProductManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductManager.Services;

namespace ProductManager.Pages.Products
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly IProductService _service;
        
        public Product Product { get; set; }
        public DeleteModel(IProductService service)
        {
            _service = service;
        }
        public void OnGet(ulong id)
        {
            if (id != null || id != 0)
            {
                Product = _service.GetByIdAsync(id).Data!;
            }
        }

        public IActionResult OnPost()
        {
            var result = _service.DeleteProduct(Product.Id);

            if (result.Result.IsSuccess)
            {
                TempData["success"] = "Product deleted successfully";
                return RedirectToPage("Index");
            }
            else 
            {
                TempData["error"] = result.Result.ErrorMessage;
                return Page();
            }

            
        }
    }
}
