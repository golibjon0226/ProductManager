using ProductManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductManager.Services;

namespace ProductManager.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _service;
        [BindProperty]
        public Product Product { get; set; }

        [BindProperty]
        public IFormFile UploadedImage { get; set; }
        public CreateModel(IProductService service)
        {
            _service = service;
        }

        public IActionResult OnPost() 
        {
            if (!ModelState.IsValid)
                return Page();

            var result = _service.CreateProduct(Product, UploadedImage);

            if (result.Result.IsSuccess)
            {
                TempData["success"] = "Product is created successfull";
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
