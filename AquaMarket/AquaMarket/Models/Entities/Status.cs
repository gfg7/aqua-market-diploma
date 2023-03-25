using AquaServer.Models.Abstract;
using System.Collections.Generic;

#nullable disable

namespace AquaServer.Models.Entities
{
    public partial class Status : MarketEntity
    {
        public Status()
        {
            OrderStatusHistories = new HashSet<OrderStatusHistory>();
        }
        public virtual ICollection<OrderStatusHistory> OrderStatusHistories { get; set; }
    }
}
