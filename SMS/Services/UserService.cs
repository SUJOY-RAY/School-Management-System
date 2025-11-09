using SMS.Infrastructure;
using SMS.Models;

namespace SMS.Services
{
    public class UserService
    {
        private readonly ICrudRepository<User> _userRepo;

        public UserService(ICrudRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public Task<User?> GetByIdAsync(int id)=> _userRepo.GetByIdAsync(id);

        public async Task UpdateAsync(User user)
        {
            await _userRepo.UpdateAsync(user);
        }
        public async Task DeleteAsync(int id)
        {
            await _userRepo.DeleteAsync(id);
        }

        public Task<User?> GetByEmailAsync(string email)
            => _userRepo.FirstOrDefaultAsync(u => u.Email == email);
    }
}
