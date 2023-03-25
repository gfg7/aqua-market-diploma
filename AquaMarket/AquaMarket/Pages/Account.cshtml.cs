using AquaMarket_DTO;
using AquaServer.Services.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AquaMarket.Pages
{
    public class AccountModel : PageModel
    {
        public Profile Profile { get; set; }
        private readonly AccountService _accountService;
        public AccountModel(AccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<ActionResult> OnGet()
        {
            Profile = await _accountService.GetProfile();
            return Page();
        }
    }
}
