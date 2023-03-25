using AquaServer.Domain.Account.Actions.Helper;
using AquaServer.Interfaces.Services;
using AquaServer.Interfaces.Services.Market;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using D = AquaMarket_DTO;

namespace AquaServer.Services.Market
{
    public class OrderService
    {
        private readonly IOrderManagement _orderManager;
        private readonly IOrderViewer _orderViewer;
        private readonly ICurrentUser _currentUser;
        public OrderService(IOrderManagement orderManager, IOrderViewer orderViewer, ICurrentUser currentUser)
        {
            _orderManager = orderManager;
            _orderViewer= orderViewer;
            _currentUser = currentUser;
        }
        public async Task<List<D.Order>> GetMyOrders(int? status)
        {
            var orders = await GetOrderAtStatus(status, _currentUser.GetEmail());

            return orders;
        }

        public async Task<D.Order> GetOrder(int orderNum)
        {
            var order = await _orderViewer.GetOrders(x => x.Id == orderNum);

            return order.FirstOrDefault();
        }

        public async Task<List<D.Order>> GetOrderAtStatus(int? status, string email = null)
        {
            var orders = await _orderViewer.GetOrders(x=> x.OrderStatusHistories.OrderByDescending(x=>x.Date).First().StatusId>1);

            if (email is not null)
            {
                orders = orders.Where(x => x.Client.Email == email).ToList();
            }

            if (status is not null)
            {
                orders = orders.Where(x => x.OrderStatusHistories.Last().Status.Id == status).ToList();
            }

            return orders;
        }
    }
}
