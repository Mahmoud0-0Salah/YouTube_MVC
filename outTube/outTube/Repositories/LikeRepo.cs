using OurTube.Repositories;
using outTube.Data;
using outTube.Models.JunctionTables;

namespace ourTube.Repositories.Interfaces
{
	public class LikeRepo : Repository<LikeVideo>, ILikeRepo
	{
		public LikeRepo(ApplicationDbContext context) : base(context)
		{
		}


	}
}
