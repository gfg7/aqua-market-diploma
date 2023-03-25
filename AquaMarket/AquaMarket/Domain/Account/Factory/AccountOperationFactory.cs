using AquaMarket_DTO;
using AquaServer.Interfaces.Services;
using AquaServer.Interfaces.Services.Account;
using System.Threading.Tasks;

namespace AquaServer.Domain.Account.Factory
{
    public class AccountOperationFactory : IBaseAccountOperation, IEmployeeAccountOperation, IClientActionOperation
    {
        private readonly IBaseAccountOperation _generalAccountOperation;
        private readonly IEmployeeAccountOperation _employeeAccountOperation;
        private readonly IClientActionOperation _clientAccountOperation;
        private readonly ICurrentUser _user;
        public AccountOperationFactory(ICurrentUser user, IBaseAccountOperation generalAccountOperation, IEmployeeAccountOperation employeeAccountOperation, IClientActionOperation clientAccountOperation)
        {
            _user = user;
            _generalAccountOperation = generalAccountOperation;
            _clientAccountOperation = clientAccountOperation;
            _employeeAccountOperation = employeeAccountOperation;
        }

        public async Task<Profile> EditProfile(Profile dto) => await _generalAccountOperation.EditProfile(dto);

        public async Task<Profile> GetProfile(string email = null) => await _generalAccountOperation.GetProfile(_user.GetEmail() ?? email);
        
        public async Task<Profile> LogIn(AuthForm dto) => await _employeeAccountOperation.LogIn(dto);

        public async Task<Profile> LogIn(string email) => await _clientAccountOperation.LogIn(email);

        public async Task SignOut(string email=null) => await _generalAccountOperation.SignOut(_user.GetEmail() ?? email);

        public async Task SignUp(Profile dto) => await _clientAccountOperation.SignUp(dto);
    }
}
