using AquaServer.Domain.Account.Actions.Helper;
using AquaServer.Extensions.Exceptions;
using AquaServer.Interfaces.Wrapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AquaServer.Extensions
{
    public class CookieService
    {
        private readonly HttpContext _context;
        public CookieService(IHttpContextWrapper wrapper)
        {
            _context = wrapper.GetContext();
        }

        public async Task Set(string email, bool remember = false, string role = null, string policy = null)
        {
            if (role is null)
            {
                role = AccessRole.Client;
            }

            var authenticationProperties = new AuthenticationProperties()
            {
                IsPersistent = remember
            };

            var claimsIdentity =
                new ClaimsIdentity(
                    new List<Claim>
                    {
                        new (ClaimsIdentity.DefaultNameClaimType, email),
                        new (ClaimsIdentity.DefaultRoleClaimType, role),
                    },
                    "ApplicationCookie",
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType
                    );

            await _context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authenticationProperties);
        }
        public async Task Check(string email)
        {
            _context.Request.Cookies.TryGetValue(".AspNetCore.Cookies", out string cookie);
            if (cookie is null)
            {
                throw new NotConfirmedUserException("Подтвердите аккаунт с этого устройства.");
            }

            if (_context.User.Identity.Name != email)
            {
                throw new NotConfirmedUserException("С этого устройства была подтверждена другая учетная запись.");
            }
        }
    }
}
