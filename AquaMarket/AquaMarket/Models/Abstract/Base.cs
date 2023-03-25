using AquaServer.Interfaces;
using AquaServer.Interfaces.Models;
using System;

namespace AquaServer.Models.Abstract
{
    public abstract class Base : IEntity
    {
        public virtual IComparable GetId()
        {
            throw new NotImplementedException();
        }
    }
}
