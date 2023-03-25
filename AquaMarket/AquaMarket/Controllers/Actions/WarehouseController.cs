using AquaMarket_DTO;
using AquaServer.Services.EmployeeRoleActions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquaServer.Controllers.Actions
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly StockmanService _stockmanService;
        public WarehouseController(StockmanService stockmanService)
        {
            _stockmanService = stockmanService;
        }

        [HttpPost]
        [Route("ready/{orderNum}")]
        public async Task<Order> Ready(int orderNum, string? comment = null) => await _stockmanService.Ready(orderNum, comment);
        
        [HttpPost]
        [Route("delivery/{orderNum}")]
        public async Task<Order> Delivery(int orderNum, string? comment = null) => await _stockmanService.Delivery(orderNum, comment);
        
        [HttpPost]
        [Route("finish/{orderNum}")]
        public async Task<Order> Finish(int orderNum, string? comment = null) => await _stockmanService.Finish(orderNum, comment);

    }
}
