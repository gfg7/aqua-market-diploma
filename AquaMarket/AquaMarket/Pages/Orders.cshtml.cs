using AquaMarket_DTO;
using AquaServer.Domain.Market.Cart;
using AquaServer.Services.Market;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AquaMarketWebFront.Pages
{
    public class OrdersModel : PageModel
    {
        private readonly OrderService _service;
        public OrdersModel(OrderService service)
        {
            _service = service;
        }

        public async Task<ActionResult> OnGetMyOrders(int? status, bool desc = true)
        {
            var orders = await _service.GetMyOrders(status);

            if (desc)
            {
                orders.Reverse();
            }

            return Partial("OrderPanel", orders);
        }

        public async Task<ActionResult> OnGetOrders(int status, bool desc = true, string? search = null)
        {
            var orders = await _service.GetOrderAtStatus(status);

            if (search is not null)
            {
                orders = orders.Where(x => 
                x.Client.Name.Contains(search) || 
                x.Client.Surname.Contains(search) || 
                (x.Client.Patronymic ?? string.Empty).Contains(search) ||
                x.Client.Email.Contains(search) ||
                x.Id.ToString().Contains(search)).ToList();
            }

            if (desc)
            {
                orders.Reverse();
            }

            return Partial("OrderPanel", orders);
        }
    }
}
