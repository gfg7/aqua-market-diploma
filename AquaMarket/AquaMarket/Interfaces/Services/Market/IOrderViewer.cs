using D = AquaMarket_DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AquaServer.Models.Entities;

namespace AquaServer.Interfaces.Services
{
    public interface IOrderViewer
    {
        Task<List<D.Order>> GetOrders(Expression<Func<Order,bool>> match);
        Task<D.Order> GetCurrentOrder(string email, bool track = true);
        Task<D.Order> GetOrderById(int orderNum);
    }
}
