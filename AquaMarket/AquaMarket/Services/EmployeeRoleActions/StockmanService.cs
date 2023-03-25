using D = AquaMarket_DTO;
using AquaServer.Interfaces.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AquaServer.Interfaces.Services.Market;
using AquaServer.Interfaces.Services;

namespace AquaServer.Services.EmployeeRoleActions
{
    public class StockmanService
    {
        private readonly IOrderViewer _orderViewer;
        private readonly IOrderManagement _orderManagement;
        public StockmanService(IOrderViewer orderViewer, IOrderManagement orderManagement)
        {
            _orderViewer = orderViewer;
            _orderManagement = orderManagement;
        }

        public async Task<D.Order> Finish(int orderNum, string comment = null)
        {
            var order = await _orderViewer.GetOrderById(orderNum);

            if (order.OrderStatusHistories.Any(x => x.Status.Id == 6))
            {
                throw new Exception("Заказ уже завершен.");
            }

            order = await _orderManagement.UpdateOrderStatus(orderNum, 6, comment);

            return order;
        }
        
        public async Task<D.Order> Ready(int orderNum, string comment = null)
        {
            var order = await _orderViewer.GetOrderById(orderNum);

            if (order.OrderStatusHistories.Any(x => x.Status.Id == 4))
            {
                throw new Exception("Заказ уже готов к выдаче.");
            }

            order = await _orderManagement.UpdateOrderStatus(orderNum, 4, comment);

            return order;
        }
        
        public async Task<D.Order> Delivery(int orderNum, string comment = null)
        {
            var order = await _orderViewer.GetOrderById(orderNum);

            if (order.OrderStatusHistories.Any(x => x.Status.Id == 5))
            {
                throw new Exception("Заказ уже доставляется.");
            }

            order = await _orderManagement.UpdateOrderStatus(orderNum, 5, comment);

            return order;
        }
    }
}
