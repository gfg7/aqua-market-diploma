using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaServer.Interfaces.Services
{
    public interface ICurrentUser
    {
        string GetEmail();
        bool IsInRole(string role);
    }
}
