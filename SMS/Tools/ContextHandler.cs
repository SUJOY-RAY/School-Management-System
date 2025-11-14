using SMS.Infrastructure.Interfaces;
using SMS.Models;
using SMS.Models.school_related;
using System.Security.Claims;

namespace SMS.Tools
{
    public class ContextHandler
    {
        private readonly IHttpContextAccessor _http;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<School> _schoolRepository;

        public ContextHandler(IHttpContextAccessor http, IGenericRepository<User> userRepository, IGenericRepository<School> schoolRepository)
        {
            _http = http;
            _userRepository = userRepository;
            _schoolRepository = schoolRepository;
        }

        public string GetCurrentUserRole()
        {
            var role = _http.HttpContext?.User?
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (role!=null)
            {
                return role;
            }
            throw new UnauthorizedAccessException("No user logged in");
        }

        public string GetCurrentUserEmail()
        {
            var email = _http.HttpContext?.User?
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (email != null) { 
                return email;
            }
            throw new UnauthorizedAccessException("No user logged in");
        }

        public int GetCurrentUserId()
        {
            var uid = _http.HttpContext?.User?
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (uid != null)
            {
                return int.Parse(uid);
            }
            throw new UnauthorizedAccessException("No user logged in");
        }

        public async Task<School?> GetCurrentUserSchool()
        {
            var schoolID = _http.HttpContext?.User?
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.GroupSid)?.Value;
            if (schoolID != null)
            {
                return await _schoolRepository.GetByIdAsync(int.Parse(schoolID));
            }
            throw new UnauthorizedAccessException("No user logged in or user doesnot have any school");
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            var currUserId = GetCurrentUserId();
            return await _userRepository.GetByIdAsync(currUserId);
        }
    }
}