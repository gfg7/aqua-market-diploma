using AquaServer.Extensions.Exceptions;
using AquaServer.Services.Account.Helper;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AquaServer.Pages
{
    public class FinishRegistrationModel : PageModel
    {
        private readonly AuthTokenService _service;
        private readonly INotyfService _notyf;
        public bool Remember { get; set; } = true;

        public FinishRegistrationModel(AuthTokenService service, INotyfService notyf)
        {
            _service = service;
            _notyf = notyf;
        }

        public async Task<ActionResult> OnGet(string encoded)
        {
            try
            {
                await _service.ConfirmAccountAsync(encoded, Remember);
                _notyf.Success("Добро пожаловать!");
                return RedirectToPage("/Catalogue");

            }
            catch (EntityNotFoundException ex)
            {
                _notyf.Error(ex.Message);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
            }
            return RedirectToPage("/Auth");
        }
    }
}
