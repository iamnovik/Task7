using Microsoft.AspNetCore.SignalR;

namespace lab7.Controllers;

public class WebRTCHub : Hub
{
    public async Task Send(Data data)
    {
        System.Console.WriteLine(data.id +" send");
        await Clients.Others.SendAsync("ReceiveMessage", data.message);
       // await Clients.Group(data.id).SendAsync("ReceiveMessage", data.message);
    }

    public async Task JoinChatGroup(string chatId)
    {
        Console.WriteLine("group"+chatId);
        //await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
    }

    public async Task LeaveChatGroup(string chatId)
    {
        //await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
    }
}

public class Data
{
    public string id { get; set; }
    public string message { get; set; }
}
