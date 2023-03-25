using AquaMarket_DTO;
using AquaServer.Domain.Account.Factory;
using System.Threading.Tasks;

namespace AquaServer.Services.Account
{
    public class AccountService
    {
        private readonly AccountOperationFactory _accountOperation;
        public AccountService(AccountOperationFactory accountOperation)
        {
            _accountOperation = accountOperation;
        }

        public async Task<Profile> EditProfile(Profile dto) => await _accountOperation.EditProfile(dto);

        public async Task<Profile> GetProfile(string email=null) => await _accountOperation.GetProfile(email);

        public async Task<Profile> LogIn(AuthForm dto) => await _accountOperation.LogIn(dto);

        public async Task<Profile> LogIn(string email) => await _accountOperation.LogIn(email);

        public async Task SignUp(Profile profile) => await _accountOperation.SignUp(profile);
        public async Task SignOut(string email=null) => await _accountOperation.SignOut(email);
    }
}
