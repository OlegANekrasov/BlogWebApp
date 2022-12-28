using BlogWebApp.DAL.EF;
using BlogWebApp.DAL.Models.Users;

namespace BlogWebApp.DAL.Repository
{
    public class UsersRepository : Repository<User>
    {
        public UsersRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
