using Microsoft.AspNetCore.SignalR;
using ourTube.Services;

namespace ourTube.Hubs
{
    public class SubscribeHub : Hub
    {
        private readonly UserSubscriberService userSubscriber;

        public SubscribeHub(UserSubscriberService userSubscriber)
        {
            this.userSubscriber = userSubscriber;
        }

        public async Task Subscribe(string userId)
        {
            string subscriberId = userSubscriber.Subscribe(userId);
            if (subscriberId != null)
                await Clients.All.SendAsync("ReceiveSubscription", userId, subscriberId);
        }
        public async Task Unsubscribe(string userId)
        {
            string subscriberId = userSubscriber.Unsubscribe(userId);
            if (subscriberId != null)
                await Clients.All.SendAsync("ReceiveUnsubscription", userId, subscriberId);
        }
    }
}
