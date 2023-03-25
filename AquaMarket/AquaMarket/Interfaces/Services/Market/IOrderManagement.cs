using AquaMarket_DTO;
using System.Threading.Tasks;

namespace AquaServer.Interfaces.Services.Market
{
    public interface IOrderManagement : IClientOrderManagement
    {
        Task<Order> UpdateOrderStatus(int orderNum, int statusId, string comment = null);
    }
}
