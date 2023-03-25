using AquaMarket_DTO;
using AquaServer.Extensions;
using AquaServer.Extensions.Exceptions;
using AquaServer.Extensions.Mappers;
using AquaServer.Extensions.ModelValidator;
using AquaServer.Interfaces.Database;
using AquaServer.Interfaces.Services;
using AquaServer.Models.Entities;
using AquaServer.Services.Account.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AquaServer.Domain.Account.Actions.Operations
{
    public class EmployeeAccountOperation : IEmployeeAccountOperation
    {
        private readonly IRepository<Employee> _repositoryEmployee;
        private readonly Map _map;
        private readonly IPasswordService _passwordService;
        private readonly AuthTokenService _tokenService;
        private readonly CookieService _cookie;

        public EmployeeAccountOperation(AuthTokenService tokenService, IRepository<Employee> repositoryEmployee, Map map, IPasswordService passwordService, CookieService cookie)
        {
            _repositoryEmployee = repositoryEmployee;
            _map = map;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _cookie = cookie;
        }

        public async Task<Profile> LogIn(AuthForm dto)
        {
            ModelValidator.Validate(dto);

            var emp = await (await _repositoryEmployee.Get()).Include(x=>x.Account).FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (emp is null)
            {
                throw new EntityNotFoundException("Сотрудник с заданным Email не найден.");
            }

            if (!_passwordService.Compare(emp.Hash, emp.Salt, dto.Password))
            {
                throw new Exception("Неверный пароль.");
            }

            try
            {
                await _cookie.Check(dto.Email);

                var profile = emp.Account;

                return _map.ToDto(profile);
            }
            catch (NotConfirmedUserException)
            {
                await _tokenService.SendAuthEmail(dto.Email);
                throw new NotConfirmedUserException("Аккаунт с заданным Email уже существует, но не подтвержден для этого устройства. Отправлено письмо для подтверждения.");
            }
        }
    }
}
