using AquaServer.Models.Abstract;
using System.Collections.Generic;

namespace AquaServer.Models.Entities
{
    public class UserAccount : Person
    {
        public UserAccount()
        {
            Orders = new HashSet<Order>();
        }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Patronymic { get; set; }
        public string? Phone { get; set; }
        public bool IsConfirmed { get; set; } = false;
        public virtual ConfirmationToken ConfirmationToken { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
