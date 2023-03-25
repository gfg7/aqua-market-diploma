using AquaMarket_DTO;
using AquaServer.Services.Market;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AquaMarket.Pages
{
    public class OrdersDetailModel : PageModel
    {
        private readonly OrderService _service;

        public Order Order { get; set; }
        public OrdersDetailModel(OrderService service)
        {
            _service = service;
        }

        public async Task<ActionResult> OnGet(int orderNum)
        {
            Order = await _service.GetOrder(orderNum);

            return Page();
        }

        public async Task<ActionResult> OnGetHistory(int orderNum)
        {
            var order = await _service.GetOrder(orderNum);
            return Partial("OrderStatusHistoryPanel", order.OrderStatusHistories);
        }
    }
}
