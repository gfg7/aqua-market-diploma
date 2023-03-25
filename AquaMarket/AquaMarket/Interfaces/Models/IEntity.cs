using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaServer.Interfaces.Models
{
    public interface IEntity
    {
        IComparable GetId();
    }
}
