using SMS.Infrastructure.Interfaces;
using SMS.Models;
using SMS.Models.school_related;
using SMS.Services.Templates;

namespace SMS.Services
{
    public class ClassroomService : GenericService<Classroom>
    {
        private readonly IGenericRepository<Classroom> _repository;
        public ClassroomService(IGenericRepository<Classroom> repository) : base(repository)
        {
        }
        public async Task DeleteAsync(int id, User currUser)
        {
            var classroom = await _repository.FirstOrDefaultAsync(
                c => c.Id == id && c.SchoolID == currUser.SchoolID);

            if (classroom != null)
            {
                await _repository.DeleteAsync(id);
            }
        }
    }
}
