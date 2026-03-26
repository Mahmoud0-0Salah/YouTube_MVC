using ourTube.Repositories.Interfaces;
using OurTube.Repositories;
using outTube.Data;
using outTube.Models.JunctionTables;

namespace ourTube.Repositories
{
    public class UserCreateCommentRepo : Repository<UserCreateComment>, IUserCreateCommentRepo
    {
        public UserCreateCommentRepo(ApplicationDbContext context) : base(context)
        {
        }
    }
}
