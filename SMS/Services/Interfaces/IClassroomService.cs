using SMS.Models.school_related;
using SMS.Shared;

namespace SMS.Services.Interfaces
{
    public interface IClassroomService
    {
        Task<Classroom> CreateAsync(ClassroomCreateDto createDto);
        Task<Classroom> UpdateAsync(ClassroomCreateDto updateDto);
        Task<Classroom> DeleteAsync(int id);
        Task<Classroom> GetByIdAsync(int id);
        Task<Classroom> GetAll(int id);
    }
}
