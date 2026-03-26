using ourTube.Repositories.Interfaces;
using OurTube.Repositories;
using OurTube.Repositories.Interfaces.Common;
using outTube.Data;
using outTube.Models.JunctionTables;

namespace ourTube.Repositories
{
    public class WatchVideoRepo : Repository<WatchVideo>, IWatchVideoRepo
    {
        public WatchVideoRepo(ApplicationDbContext context) : base(context)
        {
        }
    }
}
