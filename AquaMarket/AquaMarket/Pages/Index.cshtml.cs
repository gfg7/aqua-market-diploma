using AquaMarket_DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquaMarketWebFront.Pages
{
    public class IndexModel : PageModel
    {
        public async Task<ActionResult> OnGet()
        {
            return RedirectToPage("/Catalogue");
        }
    }
}