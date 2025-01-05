using ProductManager.Data;
using ProductManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductManager.Services;

namespace ProductManager.Pages.Products
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly IProductService _service;
        [BindProperty]
        public Product Product { get; set; }

        [BindProperty]
        public IFormFile UploadedImage { get; set; }
        public EditModel(IProductService service)
        {
            _service = service;
        }
        public void OnGet(ulong id)
        {
            if( id != null || id != 0)
            {
                Product = _service.GetByIdAsync(id).Data!;
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = _service.UpdateProduct(Product, UploadedImage);

            if (result.Result.IsSuccess)
            {
                TempData["success"] = "Product edited successfully";
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
