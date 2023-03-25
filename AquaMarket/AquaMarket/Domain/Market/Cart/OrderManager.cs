using AquaServer.Extensions.Exceptions;
using AquaServer.Extensions.Mappers;
using AquaServer.Extensions.ModelValidator;
using AquaServer.Interfaces.Database;
using AquaServer.Interfaces.Services.Market;
using AquaServer.Models.Entities;
using Microsoft.EntityFrameworkCore;
using D = AquaMarket_DTO;

namespace AquaServer.Domain.Market.Cart
{
    public class OrderManager : IOrderManagement
    {
        private readonly IRepository<OrderStatusHistory> _repositoryOrderStatusHistory;
        private readonly IRepository<Order> _repositoryOrder;
        private readonly IRepository<Content> _repositoryContent;
        private readonly IRepository<WaterProduct> _repositoryWaterProduct;
        private readonly ICartViewer _cartViewer;
        private readonly Map _map;

        public OrderManager(
            Map map,
            ICartViewer cartViewer,
            IRepository<WaterProduct> repositoryWaterProduct,
            IRepository<Content> repositoryContent,
            IRepository<Order> repositoryOrder,
            IRepository<OrderStatusHistory> repositoryOrderStatusHistory)
        {
            _map = map;
            _repositoryWaterProduct = repositoryWaterProduct;
            _cartViewer = cartViewer;
            _repositoryContent = repositoryContent;
            _repositoryOrder = repositoryOrder;
            _repositoryOrderStatusHistory = repositoryOrderStatusHistory;
        }

        private async Task<D.Order> GetCurrentOrder(string email, bool create = false)
        {
            var curOrder = await _cartViewer.GetCurrentOrder(email, false);

            if (curOrder is null)
            {
                if (create)
                {
                    var order = new Order()
                    {
                        ClientId = email
                    };

                    order = await _repositoryOrder.Create(order);

                    await _repositoryOrderStatusHistory.Create(new OrderStatusHistory()
                    {
                        Date = DateTime.Now,
                        OrderNum = order.Id,
                        StatusId = 1
                    });

                    return await GetCurrentOrder(email);
                }
                else
                {
                    throw new Exception("Ошибка создания заказа.");
                }
            }
            return curOrder;
        }

        public async Task<D.Order> UpdateOrderStatus(int orderNum, int statusId, string comment = null)
        {
            var order = (await _cartViewer.GetOrders(x => x.Id == orderNum)).FirstOrDefault();

            if (order is null)
            {
                throw new EntityNotFoundException("Заказ не найден.");
            }

            await _repositoryOrderStatusHistory.Create(new OrderStatusHistory()
            {
                Date = DateTime.Now,
                OrderNum = order.Id,
                StatusId = statusId,
                Comment = comment
            });

            if (order is null)
            {
                throw new EntityNotFoundException("Произошла ошибка при оформлении заказа.");
            }

            order = (await _cartViewer.GetOrders(x => x.Id == orderNum)).First();

            return order;
        }

        public async Task<D.Order> AddToCart(string email, string article, int count)
        {
            var content = new Content()
            {
                Article = article,
                Count = count
            };

            var curOrder = await GetCurrentOrder(email, true);

            var curContent = curOrder.Contents?.FirstOrDefault(x => x.Accessory.Article == article);

            if (curContent is null)
            {
                content.OrderNum = curOrder.Id;

                await _repositoryContent.Create(content);
            }
            else
            {
                curContent.Count += count;
                curContent.Accessory = null;

                content = _map.ToEntity(curContent);

                content.OrderNum = curOrder.Id;
                content.Article = article;
                content.Id = curContent.Id;

                await _repositoryContent.Update(content);
            }

            return await GetCurrentOrder(email);
        }

        public async Task<D.Order> RemoveFromCart(string email, string article = null, int? count = null)
        {
            var curOrder = await GetCurrentOrder(email);

            if (article is null)
            {
                await _repositoryOrder.Delete(curOrder.Id);

            }
            else
            {
                var curContent = curOrder.Contents.FirstOrDefault(x => x.Accessory.Article == article);

                if (curContent is null)
                {
                    throw new EntityNotFoundException("Позиция заказа не найдена.");
                }

                if (count is null)
                {
                    await _repositoryContent.Delete(curContent.Id);
                }
                else
                {
                    if (curContent.Count == 1)
                    {
                        throw new Exception("Количество товара 1. Удалите позицию из заказа.");
                    }

                    curContent.Count -= (int)count;
                    curContent.Accessory = null;

                    var content = _map.ToEntity(curContent);

                    content.OrderNum = curOrder.Id;
                    content.Article = article;
                    content.Id = curContent.Id;

                    await _repositoryContent.Update(content);
                }
            }

            return await GetCurrentOrder(email);
        }

        public async Task<D.Order> ConfirmPurchase(string email, string comment = null)
        {
            var curOrder = await GetCurrentOrder(email);

            return await UpdateOrderStatus(curOrder.Id, 2, comment);
        }

        public async Task<D.Order> UpdateDestinationAddress(int orderNum, int? cityId, string address = null)
        {
            var orderStatus = await (await _repositoryOrderStatusHistory.Get(false)).AnyAsync(x => x.StatusId > 5 && x.OrderNum == orderNum);

            if (orderStatus)
            {
                throw new Exception("Менять место доставки заказа можно только до стадии \"Доставка товара\".");
            }

            var order = await (await _repositoryOrder.Get(true))
                .Include(x => x.City)
                .Include(x => x.OrderStatusHistories).ThenInclude(x => x.Status)
                .Include(x => x.Contents).ThenInclude(x => x.Accessory).ThenInclude(x => x.AccessoryType)
                .Include(x => x.Client)
                .Include(x => x.WaterProducts).ThenInclude(x => x.PackageType)
                .Include(x => x.WaterProducts).ThenInclude(x => x.WaterType).FirstAsync(x => x.Id == orderNum);

            order.Address = address;
            order.CityId = cityId;

            await _repositoryOrder.Update(order);

            var orders = (await _repositoryOrder.Get(false)).Include(x => x.City)
                .Include(x => x.Client)
                .Include(x => x.WaterProducts)
                .ThenInclude(x => x.PackageType)
                .Include(x => x.WaterProducts)
                .ThenInclude(x => x.WaterType)
                .Include(x => x.Contents)
                .ThenInclude(x => x.Accessory)
                .ThenInclude(x => x.AccessoryType);

            var updated = await orders.FirstAsync(x => x.Id == orderNum);

            return _map.ToDto(updated);
        }

        public async Task<D.Order> AddToCart(string email, int count, int? water, decimal? volume, int? package, int? id)
        {
            WaterProduct waterProduct = null;
            if (id is not null)
            {
                waterProduct = await _repositoryWaterProduct.GetById(id);
                waterProduct.Count += count;

                await _repositoryWaterProduct.Update(waterProduct);

            } else
            {
                var waterProductDto = new D.WaterProductInfo()
                {
                    Count = count,
                    PackageTypeId = (int)package,
                    WaterTypeId = (int)water,
                    Volume = (decimal)volume
                };

                ModelValidator.Validate(waterProductDto);

                waterProduct = _map.ToEntity(waterProductDto);

                var curOrder = await GetCurrentOrder(email, true);
                waterProduct.OrderNum = curOrder.Id;

                await _repositoryWaterProduct.Create(waterProduct);
            }
            

            return await GetCurrentOrder(email);
        }

        public async Task<D.Order> RemoveFromCart(string email, int waterId, int? count = null)
        {
            if (count is null)
            {
                await _repositoryWaterProduct.Delete(waterId);
            }
            else
            {
                var order = await GetCurrentOrder(email);
                var waterDto = order.WaterProducts?.FirstOrDefault(x => x.Id == waterId);

                if (waterDto is null)
                {
                    throw new EntityNotFoundException("Позиция заказа не найдена.");
                }

                var water = await _repositoryWaterProduct.GetById(waterId);
                water.Count -= (int)count;

                await _repositoryWaterProduct.Update(water);
            }

            return await GetCurrentOrder(email);
        }
    }
}
