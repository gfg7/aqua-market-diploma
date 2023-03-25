using AquaMarket_DTO;
using AquaServer.Extensions.Exceptions;
using AquaServer.Services.Account;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AquaMarketWebFront.Pages
{
    public class AuthModel : PageModel
    {
        public Profile Profile { get; set; } = new();
        public AuthForm Auth { get; set; } = new();
        [BindProperty]
        [Required]
        public string ClientEmail { get; set; }

        private readonly AccountService _service;
        private readonly INotyfService _notyf;
        public AuthModel(INotyfService notyf, AccountService service)
        {
            _service = service;
            _notyf = notyf;
        }
        public void OnGet()
        {
        }

        public async Task<ActionResult> OnPostRegister(Profile profile)
        {
            try
            {
                await _service.SignUp(profile);
                _notyf.Information("Аккаунт создан. Проверьте указанную почту на наличие письма для подтверждения профиля с этого устройства.");
                return RedirectToPage("/Catalogue");
            }
            catch (NotConfirmedUserException ex)
            {
                _notyf.Information(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }

        public async Task<ActionResult> OnPostIn(AuthForm auth)
        {
            try
            {
                var result = await _service.LogIn(auth);
                return RedirectToPage("/Catalogue");
            }
            catch (NotConfirmedUserException ex)
            {
                _notyf.Information(ex.Message);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
            }
            return Page();
        }

        public async Task<ActionResult> OnPostInFast()
        {
            try
            {
                var result = await _service.LogIn(ClientEmail);
                return RedirectToPage("/Catalogue");
            }
            catch (NotConfirmedUserException ex)
            {
                _notyf.Information(ex.Message);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
            }
            return Page();
        }

        public async Task<ActionResult> OnGetSignOut()
        {
            await _service.SignOut();
            await HttpContext.SignOutAsync();
            return RedirectToPage("/Catalogue");
        }
    }
}
