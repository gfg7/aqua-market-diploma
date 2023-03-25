using AquaMarket_DTO;
using System.Threading.Tasks;

namespace AquaServer.Interfaces.Services
{
    public interface IEmployeeAccountOperation
    {
        Task<Profile> LogIn(AuthForm dto);
    }
}
