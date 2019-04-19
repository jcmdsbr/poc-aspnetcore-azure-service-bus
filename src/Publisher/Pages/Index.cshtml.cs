using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Publisher.Services.Contracts;

namespace Publisher.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }

        [BindProperty()]
        public string DescriptionProduct { get; set; }
        public async Task<IActionResult> OnPost([FromServices] ICreateNewProductService service)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await service.Add(Product.CreateNewProduct(DescriptionProduct));

            return RedirectToPage("./Index");
        }
    }
}
