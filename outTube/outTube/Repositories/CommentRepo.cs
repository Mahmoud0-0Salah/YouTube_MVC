using ourTube.Repositories.Interfaces;
using OurTube.Repositories;
using outTube.Data;
using outTube.Models;
using outTube.Models.JunctionTables;

namespace ourTube.Repositories
{
    public class CommentRepo : Repository<Comment>, ICommentRepo
    {
        public CommentRepo(ApplicationDbContext context) : base(context)
        {
        }
    }
}
