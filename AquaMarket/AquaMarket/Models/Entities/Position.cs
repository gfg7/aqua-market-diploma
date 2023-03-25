using AquaServer.Models.Abstract;
using System.Collections.Generic;

#nullable disable

namespace AquaServer.Models.Entities
{
    public partial class Position : Entity
    {
        public Position()
        {
            Employees = new HashSet<Employee>();
        }

        public string Title { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
