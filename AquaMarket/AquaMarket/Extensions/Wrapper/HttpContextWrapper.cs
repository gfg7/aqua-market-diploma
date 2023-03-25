using AquaServer.Interfaces;
using AquaServer.Interfaces.Wrapper;
using Microsoft.AspNetCore.Http;

namespace AquaServer.Extensions.Wrapper
{
    public class HttpContextWrapper : IHttpContextWrapper
    {
        private readonly HttpContext _context;
        public HttpContextWrapper(IHttpContextAccessor accessor)
        {
            _context = accessor.HttpContext;
        }

        public HttpContext GetContext() => _context;
    }
}
