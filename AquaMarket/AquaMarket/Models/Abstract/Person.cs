using AquaServer.Interfaces.Models;
using System;

namespace AquaServer.Models.Abstract
{
    public abstract class Person : Base, IEntity
    {
        private string email;

        public string Email { get => email.Trim(); set => email = value; }
        public override IComparable GetId()
        {
            return Email.Trim();
        }
    }
}
