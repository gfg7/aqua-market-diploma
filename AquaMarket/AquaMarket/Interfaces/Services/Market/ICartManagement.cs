using AquaMarket_DTO;
using System.Threading.Tasks;

namespace AquaServer.Interfaces.Services.Market
{
    public interface ICartManagement
    {
        Task<Order> AddToCart(string email, string article, int count);
        Task<Order> RemoveFromCart(string email, string article=null, int? count=null);
        Task<Order> AddToCart(string email, int count, int? water, decimal? volume, int? package, int? id);
        Task<Order> RemoveFromCart(string email, int waterId, int? count = null);
    }
}
