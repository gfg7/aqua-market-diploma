using AquaServer.Models.Abstract;
using System.Collections.Generic;

#nullable disable

namespace AquaServer.Models.Entities
{
    public partial class AccessoryType : MarketEntity
    {
        public AccessoryType()
        {
            Accessories = new HashSet<Accessory>();
        }
        public virtual ICollection<Accessory> Accessories { get; set; }
    }
}
