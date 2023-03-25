using AquaMarket_DTO;
using AquaServer.Extensions.Mappers;
using AquaServer.Extensions.ModelValidator;
using AquaServer.Interfaces.Database;
using AquaServer.Interfaces.Services.Account;
using AquaServer.Models.Entities;
using System.Threading.Tasks;

namespace AquaServer.Domain.Account.Actions.Operations
{
    public class BaseAccountOperation : IBaseAccountOperation
    {
        internal readonly IRepository<UserAccount> _repository;
        internal readonly Map _map;
        public BaseAccountOperation(Map map, IRepository<UserAccount> repository)
        {
            _map = map;
            _repository = repository;
        }

        public async Task<Profile> EditProfile(Profile dto)
        {
            ModelValidator.Validate(dto);

            var upd = _map.ToEntity(dto);
            upd = await _repository.Update(upd);

            return _map.ToDto(upd);
        }

        public async Task<Profile> GetProfile(string email) => _map.ToDto(await _repository.GetById(email));

        public async Task SignOut(string email)
        {
            var user = await _repository.GetById(email);
            user.IsConfirmed = false;
            await _repository.Update(user);
        }
    }
}
