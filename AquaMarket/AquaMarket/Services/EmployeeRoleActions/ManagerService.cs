using D = AquaMarket_DTO;
using AquaServer.Services.Market;
using System.Collections.Generic;
using System.Threading.Tasks;
using AquaServer.Interfaces.Services;
using AquaServer.Interfaces.Services.Market;
using AquaServer.Extensions.Exceptions;
using AquaServer.Models.Entities;
using AquaServer.Domain.Database;
using AquaServer.Interfaces.Database;

namespace AquaServer.Services.EmployeeRoleActions
{
    public class ManagerService
    {
        private readonly IOrderViewer _orderViewer;
        private readonly IOrderManagement _orderManagement;
        private readonly IRepository<UserAccount> _repository;
        private readonly IRepository<Employee> _repositoryEmployee;
        private readonly IPasswordService _passwordService;
        private readonly IMessagingService _messagingService;
        public ManagerService(IMessagingService messagingService,IOrderViewer orderViewer, IOrderManagement orderManagement, IRepository<UserAccount> repository, IRepository<Employee> repositoryEmployee, IPasswordService passwordService)
        {
            _messagingService = messagingService;
            _passwordService = passwordService;
            _repository = repository;
            _repositoryEmployee = repositoryEmployee;
            _orderManagement = orderManagement;
            _orderViewer = orderViewer;
        }

        public async Task<D.Order> AcceptOrder(int orderNum, string comment = null)
        {
            var order = await _orderViewer.GetOrderById(orderNum);

            if (order.OrderStatusHistories.Any(x=> x.Status.Id == 3))
            {
                throw new Exception("Заказ уже подтвержден.");
            }

            order = await _orderManagement.UpdateOrderStatus(orderNum, 3, comment);

            return order;
        }
        
        public async Task<D.Order> CancelOrder(int orderNum, string comment = null)
        {
            var order = await _orderViewer.GetOrderById(orderNum);

            if (order.OrderStatusHistories.Any(x=> x.Status.Id == 7))
            {
                throw new Exception("Заказ уже отклонен.");
            }

            order = await _orderManagement.UpdateOrderStatus(orderNum, 7, comment);

            return order;
        }

        public async Task CreateEmployee(string email, int position)
        {
            var user = await _repository.GetById(email);

            var pass = Guid.NewGuid().ToString("N")[..10];
            var (hash, salt) = _passwordService.Generate(pass);

            var account = new Employee()
            {
                Email = email,
                Hash = hash,
                Salt = salt,
                PositionId = position
            };

            await _repositoryEmployee.Create(account);

            await _messagingService.Send(email, "Пароль учетной записи", $"Ваш пароль: {pass}\n Передача пароля третьим лицам запрещена");
        }
    }
}
