using AquaServer.Models.Abstract;
using System;

#nullable disable

namespace AquaServer.Models.Entities
{
    public partial class OrderStatusHistory : Entity
    {
        public int OrderNum { get; set; }
        public DateTime Date { get; set; }
        public int StatusId { get; set; }
        public string Comment { get; set; }
        public virtual Order OrderNumNavigation { get; set; }
        public virtual Status Status { get; set; }
    }
}
