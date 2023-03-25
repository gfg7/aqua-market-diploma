using AquaServer.Extensions.Exceptions;
using AquaServer.Extensions.Mappers;
using AquaServer.Interfaces.Database;
using AquaServer.Interfaces.Services;
using AquaServer.Interfaces.Services.Market;
using AquaServer.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using D = AquaMarket_DTO;

namespace AquaServer.Domain.Market.Cart
{
    public class OrderViewer : ICartViewer, IOrderViewer
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly Map _map;
        public OrderViewer(IRepository<Order> orderRepository, Map map)
        {
            _map = map;
            _orderRepository = orderRepository;
        }

        public async Task<D.Order> GetCurrentOrder(string email, bool track = true)
        {
            var orders =  (await _orderRepository.Get(track))
                .Include(x=>x.City)
                .Include(x => x.OrderStatusHistories).ThenInclude(x=>x.Status)
                .Include(x => x.Contents).ThenInclude(x => x.Accessory).ThenInclude(x=>x.AccessoryType)
                .Include(x => x.Client)
                .Include(x => x.WaterProducts).ThenInclude(x => x.PackageType)
                .Include(x => x.WaterProducts).ThenInclude(x => x.WaterType);

            var order = await orders
                .FirstOrDefaultAsync(x =>
            x.Client.Email == email &&
            x.OrderStatusHistories.OrderByDescending(y => y.Date).First().StatusId == 1
            );

            return order is null ? null : _map.ToDto(order);
        }

        public async Task<List<D.Order>> GetOrders(Expression<Func<Order, bool>> match=null)
        {
            var orders = (await _orderRepository.Get(false))
                  .Include(x => x.City)
                  .Include(x => x.OrderStatusHistories).ThenInclude(x => x.Status)
                  .Include(x => x.Contents).ThenInclude(x => x.Accessory).ThenInclude(x => x.AccessoryType)
                  .Include(x => x.Client)
                  .Include(x => x.WaterProducts).ThenInclude(x => x.PackageType)
                  .Include(x => x.WaterProducts).ThenInclude(x => x.WaterType).AsQueryable();

            if (match is not null)
            {
                orders = orders.Where(match);
            }

            return orders.Select(x => _map.ToDto(x)).ToList();
        }

        public async Task<D.Order> GetOrderById(int orderNum)
        {
            var order = (await GetOrders(x => x.Id == orderNum)).FirstOrDefault();

            if (order is null)
            {
                throw new EntityNotFoundException("Заказ не найден");
            }

            return order;
        }
    }
}
