using SMS.Models.user_lists;

namespace SMS.Infrastructure
{
    public class ListOfUsersRepository : CrudRepository<ListOfUsers>
    {
        public ListOfUsersRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
