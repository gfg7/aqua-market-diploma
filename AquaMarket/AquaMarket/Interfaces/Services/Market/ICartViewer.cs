using AquaMarket_DTO;
using System.Threading.Tasks;

namespace AquaServer.Interfaces.Services.Market
{
    public interface ICartViewer :IOrderViewer
    {
        Task<Order> GetCurrentOrder(string email, bool track = true);
    }
}
