using AquaServer.Interfaces.Models;
using System;

namespace AquaServer.Models.Abstract
{
    public abstract class Entity : Base, IEntity
    {
        public int Id { get; set; }

        public override IComparable GetId()
        {
            return Id;
        }
    }
}
