using AquaServer.Services.Account.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AquaServer.Controllers.Account
{
    [Route("api/auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly AuthTokenService _token;
        public AuthController(AuthTokenService token)
        {
            _token = token;
        }

        [HttpGet]
        [Route("finish/{encoded}")]
        public async Task ConfirmAccount(string encoded, bool remember = false) => await _token.ConfirmAccountAsync(encoded, remember);
    }
}
