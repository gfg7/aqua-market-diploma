using AquaServer.Domain.Account.Actions.Helper;
using AquaServer.Extensions;
using AquaServer.Extensions.Exceptions;
using AquaServer.Extensions.Helper;
using AquaServer.Interfaces.Database;
using AquaServer.Interfaces.Services;
using AquaServer.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AquaServer.Services.Account.Helper
{
    public class AuthTokenService
    {
        private readonly IRepository<UserAccount> _repository;
        private readonly IRepository<ConfirmationToken> _repositoryConfirmationToken;
        private readonly CookieService _cookieService;
        private readonly IMessagingService _message;
        private readonly WWWRootResources _resources;
        private readonly string _url;

        public AuthTokenService(UrlBuilder url, WWWRootResources resources, IMessagingService message, IRepository<UserAccount> repository, IRepository<ConfirmationToken> repositoryConfirmationToken, CookieService cookieService)
        {
            _url = url.BuildUrl("FinishRegistration?encoded=");
            _resources = resources;
            _message = message;
            _cookieService = cookieService;
            _repository = repository;
            _repositoryConfirmationToken = repositoryConfirmationToken;
        }

        public async Task SendAuthEmail(string email)
        {
            string encodedToken = await CreateConfirmationToken(email);

            string messageText = await _resources.ReadResource("static/email-register/index.html");

            await _message.Send(email, "Aqua Market: Подтверждение аккаунта на устройстве", messageText.Replace("AUTH_URL", _url + encodedToken));
        }

        public async Task ConfirmAccountAsync(string encoded, bool remember=false)
        {
            var data = Encoding.ASCII.GetString(Convert.FromBase64String(encoded)).Split(":");
            string email = data[0], confirm = data[1];

            var acc = await (await _repository.Get()).Include(x=>x.ConfirmationToken).Include(x=>x.Employee).FirstOrDefaultAsync(x=>x.Email == email);

            if (acc is null)
            {
                throw new EntityNotFoundException("Пользователь с данным Email не найден.");
            }

            if (acc.IsConfirmed)
            {
                throw new Exception("Аккаунт уже подтвержден.");
            } else if (acc.ConfirmationToken is null)
            {
                throw new Exception("Требуется авторизоваться еще раз.");
            }

            var confirmation = acc.ConfirmationToken;

            if (confirmation.Expire < DateTime.Now)
            {
                throw new Exception("Срок токена подтверждения истек.");
            }

            if (confirmation.Token != confirm)
            {
                throw new Exception("Невалидный токен.");
            }

            acc.IsConfirmed = true;

            await _repository.Update(acc);

            await _repositoryConfirmationToken.Delete(email);

            if (acc.Employee is not null)
            {
                if (acc.Employee.PositionId == 1)
                {
                await _cookieService.Set(email, remember, AccessRole.Manager);
                } else if (acc.Employee.PositionId == 2)
                {
                    await _cookieService.Set(email, remember, AccessRole.Warehouse);

                } else
                {
                    await _cookieService.Set(email, remember, AccessRole.Delivery);

                }
            }
            else
            {
                await _cookieService.Set(email, remember);
            }
        }

        public async Task<string> CreateConfirmationToken(string email)
        {
            var user = await _repository.GetById(email);
            if (user is null)
            {
                throw new EntityNotFoundException("Пользователь с данным Email не найден.");
            }

            string token = Guid.NewGuid().ToString("N");
            string encoded = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Join(":", new[] { email, token })));

            try
            {
                var oldConfirm = await _repositoryConfirmationToken.GetById(email);

                oldConfirm.Token = token;
                oldConfirm.Expire = DateTime.Now.AddDays(1);

                await _repositoryConfirmationToken.Update(oldConfirm);
            }
            catch
            {
                await _repositoryConfirmationToken.Create(new()
                {
                    Email = email,
                    Token = token,
                    Expire = DateTime.Now.AddDays(1)
                });
            }

            user.IsConfirmed = false;
            await _repository.Update(user);

            return encoded;
        }
    }
}
