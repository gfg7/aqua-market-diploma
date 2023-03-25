using AquaServer.Interfaces;
using AquaServer.Interfaces.Services;
using AquaServer.Interfaces.Wrapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AquaServer.Services.Account.Helper
{
    public class CurrentUser: ICurrentUser
    {
        private readonly ClaimsPrincipal _user;
        public CurrentUser(IHttpContextWrapper context)
        {
            _user = context.GetContext().User;
        }

        public string GetEmail() => _user.Identity.Name;
        public bool IsInRole(string role)=> _user.IsInRole(role);
    }
}
