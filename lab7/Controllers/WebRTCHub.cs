using Microsoft.AspNetCore.SignalR;

namespace lab7.Controllers;

public class WebRTCHub : Hub
{
    public async Task Send(Data data)
    {
        await Clients.Group(data.id.ToString()).SendAsync("ReceiveMessage", data.message);
    }

    public async Task JoinChatGroup(string chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
    }

    public async Task LeaveChatGroup(string chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
    }
}

public class Data
{
    public string id { get; set; }
    public string message { get; set; }
}
