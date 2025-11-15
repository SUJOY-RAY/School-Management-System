using SMS.Infrastructure.Interfaces;
using SMS.Models;
using SMS.Models.school_related;
using SMS.Services.Templates;
using SMS.Shared.Classroom;

namespace SMS.Services
{
    public class ClassroomService : GenericService<Classroom>
    {
        private readonly IGenericRepository<Classroom> clsRepository;
        private readonly IGenericRepository<User> userRepository;
        public ClassroomService(IGenericRepository<Classroom> repository, IGenericRepository<User> userRepository) : base(repository)
        {
            this.clsRepository = repository;
            this.userRepository = userRepository;
        }


        public async Task<IEnumerable<Classroom>> GetAllAsync(User user)
        {
            var classes = new List<Classroom>();
            switch (user.Role)
            {
                case Role.Principal:
                    classes.AddRange(await clsRepository.GetFilteredAsync(c => c.SchoolID == user.SchoolID));
                    break;
                case Role.Teacher:
                case Role.Student:
                    classes.AddRange(await clsRepository.GetFilteredAsync(c => c.SchoolID == user.SchoolID && user.Classrooms.Any(uc => uc.Id == c.Id)));
                    break;
            }
            return classes;
        }

        public async Task DeleteAsync(int id, User currUser)
        {
            var classroom = await clsRepository.FirstOrDefaultAsync(
                c => c.Id == id && c.SchoolID == currUser.SchoolID);

            if (classroom != null)
            {
                await clsRepository.DeleteAsync(id);
            }
        }
        public async Task UpdateAsync(
            int id,
            ClassroomUpdateDto classroomUpdateDto,
            User currUser
        )
        {
            var classroom = await clsRepository.GetByIdAsync(id, c => c.Attendances, c => c.Users);

            if (classroom == null)
                return;

            classroom.Name = classroomUpdateDto.Name;

            if (classroomUpdateDto.UserIds != null)
            {
                var usersToRemove = classroom.Users
                    .Where(u => !classroomUpdateDto.UserIds.Contains(u.Id))
                    .ToList();

                foreach (var u in usersToRemove)
                {
                    classroom.Users.Remove(u);
                }

                foreach (var userId in classroomUpdateDto.UserIds)
                {
                    if (!classroom.Users.Any(u => u.Id == userId))
                    {
                        var user = await userRepository.GetByIdAsync(userId);
                        if (user != null)
                            classroom.Users.Add(user);
                    }
                }
            }

            if (classroomUpdateDto.AttendanceIdsToRemove != null)
            {
                var attsToRemove = classroom.Attendances
                    .Where(a => classroomUpdateDto.AttendanceIdsToRemove.Contains(a.Id))
                    .ToList();

                foreach (var att in attsToRemove)
                {
                    classroom.Attendances.Remove(att);
                }
            }

            await clsRepository.UpdateAsync(classroom);
        }


        public async Task<IEnumerable<User>> Users(Classroom classroom)
        {

            return classroom?.Users ?? Enumerable.Empty<User>();
        } 
    }
}
