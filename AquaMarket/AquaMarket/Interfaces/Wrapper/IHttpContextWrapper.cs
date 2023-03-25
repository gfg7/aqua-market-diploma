using Microsoft.AspNetCore.Http;

namespace AquaServer.Interfaces.Wrapper
{
    public interface IHttpContextWrapper
    {
        HttpContext GetContext();
    }
}
