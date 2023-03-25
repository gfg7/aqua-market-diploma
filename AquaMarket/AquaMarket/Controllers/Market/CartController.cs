using AquaMarket_DTO;
using AquaServer.Domain.Account.Actions.Helper;
using AquaServer.Services.Market;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AquaServer.Controllers.Market
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles =nameof(AccessRole.Client))]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;
        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<Order> GetCart() => await _cartService.GetCart();

        [HttpPost]
        [Route("accessory")]
        public async Task<Order> AddAccessory(string article, int count) => await _cartService.AddToCart(article, count);

        [HttpPost]
        [Route("water")]
        public async Task<Order> AddWaterProduct(int count, int? water, decimal? volume, int? package, int? id) => await _cartService.AddToCart(count, water, volume, package,id);

        [HttpDelete]
        [Route("accessory")]
        public async Task<Order> RemoveAccessory(string article = null, int? count=null) => await _cartService.RemoveFromCart(article,count);

        [HttpDelete]
        [Route("water")]
        public async Task<Order> RemoveWaterProduct(int id, int? count = null) => await _cartService.RemoveFromCart(id, count);

        [HttpPut]
        [Route("address")]
        public async Task<Order> SetDestinationAddress(int? cityId = null, string? address = null, int? order=null) => await _cartService.UpdateDestinationAddress(cityId, address, order);

        [HttpPost]
        [Route("purchase")]
        public async Task<Order> Buy(string? comment = null) => await _cartService.ConfirmPurchase(comment);

        [HttpDelete]
        [Route("cancel/{orderNum}")]
        public async Task<Order> Cancel(int orderNum, string? comment= null) => await _cartService.Cancel(orderNum, comment);
    }
}
