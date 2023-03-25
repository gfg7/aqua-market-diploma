using System;

namespace AquaServer.Extensions.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message="Не найдено.") : base(message)
        {

        }
    }
}
