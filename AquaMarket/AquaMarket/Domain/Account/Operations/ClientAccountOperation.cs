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
using System.Linq;
using System.Threading.Tasks;

namespace AquaServer.Domain.Account.Actions.Operations
{
    public class ClientAccountOperation : IClientActionOperation
    {
        private readonly IRepository<UserAccount> _repository;
        private readonly Map _map;
        private readonly CookieService _cookie;
        private readonly AuthTokenService _tokenService;
        public ClientAccountOperation(IRepository<UserAccount> repository, AuthTokenService tokenService, Map map, CookieService token)
        {
            _repository = repository;
            _cookie = token;
            _map = map;
            _tokenService = tokenService;
        }

        public async Task SignUp(Profile dto)
        {
            ModelValidator.Validate(dto);

            UserAccount user = null;

            try
            {
                user = await _repository.GetById(dto.Email);
            }
            catch { }

            if (user is null)
            {
                var created = await _repository.Create(_map.ToEntity(dto));

                if (created is null)
                {
                    throw new Exception("Ошибка регистрации.");
                }
                else
                {
                    await _tokenService.SendAuthEmail(dto.Email);
                }
            }
            else
            {
                if (user.IsConfirmed)
                {
                    throw new Exception("Аккаунт с заданным Email уже существует");
                }
                else
                {
                    await _tokenService.SendAuthEmail(dto.Email);

                    throw new NotConfirmedUserException();
                }
            }
        }

        public async Task<Profile> LogIn(string email)
        {
            if (string.IsNullOrEmpty(email.Trim()))
            {
                throw new Exception("Заполните графу Email.");
            }

            try
            {
                var user = await (await _repository.Get()).Include(x => x.Employee).FirstOrDefaultAsync(x => x.Email == email);

                if (user is null)
                {
                    throw new EntityNotFoundException("Пользователь с заданным Email не найден.");
                }

                if (user.Employee is not null)
                {
                    throw new Exception("Требуется полная авторизация для данного аккаунта.");
                }

                await _cookie.Check(email);

                return _map.ToDto(user);
            }
            catch (NotConfirmedUserException)
            {
                await _tokenService.SendAuthEmail(email);
                throw new NotConfirmedUserException();
            }
        }
    }
}
