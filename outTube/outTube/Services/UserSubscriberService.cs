using ourTube.Repositories.Interfaces;
using outTube.Models.JunctionTables;
using System.Security.Claims;
using System.Linq;

namespace ourTube.Services
{
    public class UserSubscriberService
    {
        private readonly IUserSubscriberRepo userSubscriberRepo;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserSubscriberService(IUserSubscriberRepo userSubscriberRepo,
                                     IHttpContextAccessor httpContextAccessor)
        {
            this.userSubscriberRepo = userSubscriberRepo;
            this.httpContextAccessor = httpContextAccessor;
        }

        public string Subscribe(string userId)
        {
            string subscriberId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)?.ToString();
            if (subscriberId == null) return null;
            UserSubscriber subscriber = new UserSubscriber()
            {
                UserId = userId,
                SubscriberId = subscriberId,
                SubscribedAt = DateTime.Now
            };
            userSubscriberRepo.Create(subscriber);
            userSubscriberRepo.Save();
            return subscriberId;
        }

        public string Unsubscribe(string userId)
        {
            string subscriberId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)?.ToString();
            if (subscriberId == null) return null;
            UserSubscriber subscriber = userSubscriberRepo.GetByCondition(s => s.UserId == userId && s.SubscriberId == subscriberId).FirstOrDefault();
            if (subscriber != null)
            {
                userSubscriberRepo.Delete(subscriber);
                userSubscriberRepo.Save();
            }
            return subscriberId;
        }
    }
}
