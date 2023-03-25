using AquaMarket_DTO;
using AquaServer.Domain.Account.Actions.Helper;
using AquaServer.Services.EmployeeRoleActions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AquaServer.Controllers.Actions
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(AccessRole.Manager))]
    public class ManagementController : ControllerBase
    {
        private readonly ManagerService _managerService;
        public ManagementController(ManagerService managerService)
        {
            _managerService = managerService;
        }

        [HttpDelete]
        [Route("cancel/{orderNum}")]
        public async Task<Order> CancelOrder(int orderNum, string? comment = null) => await _managerService.CancelOrder(orderNum, comment);

        [HttpPost]
        [Route("accept/{orderNum}")]
        public async Task<Order> AcceptOrder(int orderNum, string? comment = null) => await _managerService.AcceptOrder(orderNum, comment);

        [HttpPost]
        [Route("hire")]
        public async Task Hire(string email, int position) => await _managerService.CreateEmployee(email, position);
    }
}
