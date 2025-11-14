using SMS.Infrastructure.Interfaces;
using SMS.Models;
using SMS.Models.school_related;
using SMS.Services.Templates;
using SMS.Shared.Classroom;

namespace SMS.Services
{
    public class ClassroomService : GenericService<Classroom>
    {
        private readonly IGenericRepository<Classroom> _repository;

        public ClassroomService(IGenericRepository<Classroom> repository) : base(repository)
        {
            _repository = repository;
        }
        
        public async Task<IEnumerable<Classroom>> GetAllAsync(User user)
        {
            var classes = new List<Classroom>();
            switch (user.Role)
            {
                case Role.Principal:
                    classes.AddRange(await _repository.GetFilteredAsync(c => c.SchoolID == user.SchoolID));
                    break;
                case Role.Teacher:
                case Role.Student:
                    classes.AddRange(await _repository.GetFilteredAsync(c => c.SchoolID == user.SchoolID && user.Classrooms.Any(uc => uc.Id == c.Id)));
                    break;
            }
            return classes;
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
        public async Task UpdateAsync(int id, ClassroomUpdateDto classroomUpdateDto, User currUser)
        {
            var classroom = await _repository.FirstOrDefaultAsync(
                c => c.Id == id && c.SchoolID == currUser.SchoolID);

            if (classroom != null)
            {
                classroom.Name = classroomUpdateDto.Name;
                await _repository.UpdateAsync(classroom);
            }
        }
    }
}
