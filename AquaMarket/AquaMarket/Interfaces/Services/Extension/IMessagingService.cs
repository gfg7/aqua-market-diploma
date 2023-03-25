using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaServer.Interfaces.Services
{
    public interface IMessagingService
    {
        Task Send(string to, string subject, string text, string[] attachments = null);
    }
}
