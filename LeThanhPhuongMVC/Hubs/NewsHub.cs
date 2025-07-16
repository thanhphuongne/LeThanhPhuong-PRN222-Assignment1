using Microsoft.AspNetCore.SignalR;

namespace LeThanhPhuongMVC.Hubs
{
    public class NewsHub : Hub
    {
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendTestMessage(string message)
        {
            await Clients.Group("NewsUpdates").SendAsync("TestMessage", new
            {
                Message = message,
                Sender = Context.ConnectionId,
                Timestamp = DateTime.Now
            });
        }

        public override async Task OnConnectedAsync()
        {
            // Add user to general news group when they connect
            await Groups.AddToGroupAsync(Context.ConnectionId, "NewsUpdates");

            // Send welcome message to the connected client
            await Clients.Caller.SendAsync("Connected", new {
                Message = "Successfully connected to News Hub",
                ConnectionId = Context.ConnectionId,
                Timestamp = DateTime.Now
            });

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Remove user from all groups when they disconnect
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "NewsUpdates");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
