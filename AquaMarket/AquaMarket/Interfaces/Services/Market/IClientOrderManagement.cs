using AquaMarket_DTO;
using AquaServer.Interfaces.Services.Market;
using System.Threading.Tasks;

namespace AquaServer.Interfaces.Services
{
    public interface IClientOrderManagement : ICartManagement
    {
        Task<Order> ConfirmPurchase(string email, string comment = null);
        Task<Order> UpdateDestinationAddress(int orderNum, int? cityId, string address=null);
    }
}
