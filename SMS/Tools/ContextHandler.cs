using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace SMS.Tools
{
    public class ContextHandler
    {
        private readonly IHttpContextAccessor _http;

        public ContextHandler(IHttpContextAccessor httpContextAccessor)
        {
            _http = httpContextAccessor;
        }
        public string? GetCurrentUserRole()
        {
            return _http.HttpContext?.User?
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        }

        public string? GetCurrentUserEmail()
        {
            return _http.HttpContext?.User?
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        }
    }
}