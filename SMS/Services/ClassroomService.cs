using SMS.Infrastructure;
using SMS.Models;
using SMS.Models.school_related;
using SMS.Services.Interfaces;
using SMS.Shared;
using SMS.Tools;

namespace SMS.Services
{
    public class ClassroomService :IClassroomService
    {
        private readonly ICrudRepository<Classroom> clsRepo;
        private readonly ICrudRepository<User> userRepo;

        private readonly ContextHandler handler;


        public ClassroomService(ICrudRepository<Classroom> clsRepo, ContextHandler handler)
        {
            this.clsRepo = clsRepo;
            this.handler = handler;
        }

        public async Task<Classroom> CreateAsync(ClassroomCreateDto createDto)
        {
            var currUser = await userRepo.FirstOrDefaultAsync(u => u.Email == handler.GetCurrentUserEmail());
            var newCls = new Classroom
            {
                School = currUser.School,
                Name = createDto.Name,
            };
            return await clsRepo.AddAsync(newCls);
        }

        public Task<Classroom> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Classroom> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Classroom> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Classroom> UpdateAsync(ClassroomCreateDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
