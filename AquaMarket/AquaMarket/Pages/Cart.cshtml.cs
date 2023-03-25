using AquaMarket_DTO;
using AquaServer.Services.Market;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AquaMarketWebFront.Pages
{
    public class CartModel : PageModel
    {
        public Order Order { get; set; } = new();
        private readonly CartService _cartService;
        public CartModel(CartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<ActionResult> OnGetCart()
        {
            Order = await _cartService.GetCart(null,false);
            return Partial("CartContentPanel", Order);
        }
    }
}
