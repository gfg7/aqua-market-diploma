using AquaMarket_DTO;
using System.Threading.Tasks;

namespace AquaServer.Interfaces.Services
{
    public interface IClientActionOperation
    {
        Task<Profile> LogIn(string email);
        Task SignUp(Profile profile);
    }
}
