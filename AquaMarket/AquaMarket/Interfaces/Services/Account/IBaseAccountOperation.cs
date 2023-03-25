using AquaMarket_DTO;
using System.Threading.Tasks;

namespace AquaServer.Interfaces.Services.Account
{
    public interface IBaseAccountOperation
    {
        Task<Profile> EditProfile(Profile dto);
        Task<Profile> GetProfile(string email = null);
        Task SignOut(string email = null);
    }
}
