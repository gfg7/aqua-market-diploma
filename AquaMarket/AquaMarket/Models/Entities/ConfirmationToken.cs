using AquaServer.Models.Abstract;
using System;

namespace AquaServer.Models.Entities
{
    public partial class ConfirmationToken : Person
    {
        public ConfirmationToken()
        {
        }

        public DateTime Expire { get; set; }
        public string Token { get; set; }
        public virtual UserAccount Account { get; set; }

    }
}
