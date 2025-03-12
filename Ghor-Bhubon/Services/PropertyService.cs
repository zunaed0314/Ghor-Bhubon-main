using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore;
using Ghor_Bhubon.Data;
using Ghor_Bhubon.Models;
using Ghor_Bhubon.Hubs;


public class PropertyService
{
    private readonly IHubContext<PendingPostHub> _hubContext;
    private readonly ApplicationDbContext _dbContext;

    public PropertyService(IHubContext<PendingPostHub> hubContext, ApplicationDbContext dbContext)
    {
        _hubContext = hubContext;
        _dbContext = dbContext;
    }

    public async Task AddPropertyAsync(PropertyPending property)
    {
        // Add the property to the PropertyPending table
        await _dbContext.PropertyPending.AddAsync(property);
        await _dbContext.SaveChangesAsync();

        // Get the updated count of pending properties
        int pendingPostCount = _dbContext.PropertyPending.Count();

        // Notify all connected clients (admins) with the updated count
        await _hubContext.Clients.All.SendAsync("ReceivePendingPostCount", pendingPostCount);
    }
}
