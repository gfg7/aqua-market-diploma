using AquaMarket_DTO;
using AquaServer.Domain.Account.Actions.Helper;
using AquaServer.Domain.Market.Cart;
using AquaServer.Interfaces.Services;
using AquaServer.Interfaces.Services.Market;
using AquaServer.Properties;
using System.Threading.Tasks;
using D = AquaMarket_DTO;

namespace AquaServer.Services.Market
{
    public class CartService
    {
        private readonly IOrderManagement _orderManager;
        private readonly ICartViewer _cartViewer;
        private readonly ICurrentUser _currentUser;
        public CartService(IOrderManagement orderManager, ICartViewer cartViewer, ICurrentUser currentUser)
        {
            _orderManager = orderManager;
            _cartViewer = cartViewer;
            _currentUser = currentUser;
        }

        public async Task<D.Order> AddToCart(int count, int? water, decimal? volume, int? package, int? id, string email = null) => await _orderManager.AddToCart(email ?? _currentUser.GetEmail(), count, water, volume, package, id);

        public Task<D.Order> AddToCart(string article, int count, string email = null) => _orderManager.AddToCart(email ?? _currentUser.GetEmail(), article, count);

        public Task<D.Order> ConfirmPurchase(string comment = null, string email = null) => _orderManager.ConfirmPurchase(email ?? _currentUser.GetEmail(), comment);

        public async Task<D.Order> GetCart(string email = null, bool track = true) => await _cartViewer.GetCurrentOrder(email ?? _currentUser.GetEmail(), track);

        public async Task<D.Order> RemoveFromCart(string article=null, int? count=null, string email = null) => await _orderManager.RemoveFromCart(email ?? _currentUser.GetEmail(), article, count);

        public async Task<D.Order> RemoveFromCart(int id, int? count = null, string email = null) => await _orderManager.RemoveFromCart(email ?? _currentUser.GetEmail(), id,count);

        public async Task<D.Order> UpdateDestinationAddress(int? cityId = null, string address=null, int? orderNum=null, string email = null)
        {
            Order order = null;

            if (orderNum is null)
            {
                order = await _cartViewer.GetCurrentOrder(email ?? _currentUser.GetEmail());

            } else
            {
                order = (await _cartViewer.GetOrders(x => x.Id == orderNum)).FirstOrDefault();
            }

            if (email is not null && email != _currentUser.GetEmail())
            {
                throw new Exception("Нет прав на редактирование заказа.");
            }

           return await _orderManager.UpdateDestinationAddress(order.Id, cityId, address);
        }

        public async Task<D.Order> Cancel(int orderNum, string comment=null)
{
            var order = await _cartViewer.GetOrderById(orderNum);

            if (order.OrderStatusHistories.Any(x => x.Status.Id == 8))
            {
                throw new Exception("Заказ уже отклонен.");
            }

            order = await _orderManager.UpdateOrderStatus(orderNum, 8, comment);

            return order;
        }
    }
}
