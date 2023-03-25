using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaServer.Domain.Account.Actions.Helper
{
    public static class AccessRole
    {
        public static string Client = nameof(Client);
        public static string Manager = nameof(Manager);
        public static string Warehouse = nameof(Warehouse);
        public static string Delivery = nameof(Delivery);
    }
}
