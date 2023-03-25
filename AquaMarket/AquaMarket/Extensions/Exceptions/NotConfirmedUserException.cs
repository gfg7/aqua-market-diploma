using System;

namespace AquaServer.Extensions.Exceptions
{
    public class NotConfirmedUserException : Exception
    {
        public NotConfirmedUserException(string message= "Аккаунт с заданным Email уже существует, но не подтвержден для этого устройства. Отправлено письмо для подтверждения.") : base(message)
        {

        }
    }
}
