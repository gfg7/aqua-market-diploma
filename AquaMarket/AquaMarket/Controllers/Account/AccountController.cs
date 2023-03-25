using AquaMarket_DTO;
using AquaServer.Domain.Account.Actions.Helper;
using AquaServer.Services.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AquaServer.Controllers.Account
{
    [Route("api/account")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _service;

        public AccountController(AccountService service)
        {
            _service = service;
        }

        [HttpPut]
        [Route("profile")]
        public async Task<Profile> EditProfile(Profile dto) => await _service.EditProfile(dto);

        [HttpGet]
        [Route("profile")]
        public async Task<Profile> GetProfile(string? email=null) => await _service.GetProfile(email);

        [HttpPost]
        [Route("in")]
        [AllowAnonymous]
        public async Task<Profile> LogIn(AuthForm dto) => await _service.LogIn(dto);

        [HttpPost]
        [AllowAnonymous]
        [Route("in/fast")]
        public async Task<Profile> LogIn(string email) => await _service.LogIn(email);

        [HttpGet]
        [Route("out")]
        public async Task LogOut()
        {
            await _service.SignOut();
            await HttpContext.SignOutAsync();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("up")]
        public async Task SignUp(Profile profile) => await _service.SignUp(profile);
    }
}
