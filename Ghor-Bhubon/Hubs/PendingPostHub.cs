using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Ghor_Bhubon.Hubs
{
    public class PendingPostHub : Hub
    {
        public async Task NotifyPendingPostUpdate(int totalPending)
        {
            await Clients.All.SendAsync("ReceivePendingPostUpdate", totalPending);
        }
    }
}
