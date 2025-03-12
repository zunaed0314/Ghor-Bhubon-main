using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Ghor_Bhubon.Models;

namespace Ghor_Bhubon.Hubs
{
    public class PropertyHub : Hub
    {
        // Method to notify new property addition
        public async Task NotifyNewPropertyAdded(Flat newProperty)
        {
            await Clients.All.SendAsync("NewPropertyAdded", newProperty);
        }
    }
}