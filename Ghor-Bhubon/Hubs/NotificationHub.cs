using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Ghor_Bhubon.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendPendingPostUpdate(int pendingPosts)
        {
            await Clients.All.SendAsync("ReceivePendingPostUpdate", pendingPosts);
        }
    }
}
