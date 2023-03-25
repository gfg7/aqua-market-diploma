using AquaServer.Interfaces.Wrapper;
using Microsoft.AspNetCore.Http;

namespace AquaServer.Extensions.Helper
{
    public class UrlBuilder
    {
        private readonly HttpContext _context;
        public UrlBuilder(IHttpContextWrapper wrapper)
        {
            _context = wrapper.GetContext();
        }

        public string GetBaseUrl => $"{_context.Request.Scheme}://{_context.Request.Host}/";

        public string BuildUrl(string route) => GetBaseUrl + route;
    }
}
