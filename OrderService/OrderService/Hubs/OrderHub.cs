using Microsoft.AspNetCore.SignalR;
using OrderService.Store;

namespace OrderService.Hubs
{
    public class OrderHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            // Add the user to a group
            await Groups.AddToGroupAsync(Context.ConnectionId, "ChatimeSummarecon");
            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task AddOrder(Order order)
        {
            await Clients.Group("ChatimeSummarecon").SendAsync("ReceiveOrder", order);
        }
    }
}