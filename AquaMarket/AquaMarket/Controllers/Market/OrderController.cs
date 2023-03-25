using AquaMarket_DTO;
using AquaServer.Domain.Account.Actions.Helper;
using AquaServer.Services.Market;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquaServer.Controllers.Market
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _service;
        public OrderController(OrderService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = nameof(AccessRole.Client))]
        public async Task<List<Order>> GetMyOrders(int? status) => await _service.GetMyOrders(status);

        [HttpGet]
        [Authorize(Roles = $"{nameof(AccessRole.Manager)},{nameof(AccessRole.Delivery)},{nameof(AccessRole.Warehouse)}")]
        [Route("order/{orderNum}")]
        public async Task<Order> GetOrder(int orderNum) => await _service.GetOrder(orderNum);


        [HttpGet]
        [Route("status/{status}")]
        [Authorize(Roles = $"{nameof(AccessRole.Manager)},{nameof(AccessRole.Delivery)},{nameof(AccessRole.Warehouse)}")]
        public async Task<List<Order>> GetOrdersAtStatus(int status, string email = null) => await _service.GetOrderAtStatus(status, email);
    }
}
