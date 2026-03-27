using ourTube.Repositories.Interfaces;
using OurTube.Repositories;
using OurTube.Repositories.Interfaces.Common;
using outTube.Data;
using System.Linq.Expressions;

namespace ourTube.Repositories
{
    public class UserSubscriberRepo : Repository<UserSubscriber>, IUserSubscriberRepo
    {
        public UserSubscriberRepo(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}
