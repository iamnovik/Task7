using Microsoft.AspNetCore.SignalR;

namespace lab7.Controllers;

public class WebRTCHub : Hub
{
    /*public async Task Send(string message)
    {
        System.Console.WriteLine(" send");
        await Clients.Others.SendAsync("Receive", message);
    }*/
    
    public async Task Send(Data data)
    {
        System.Console.WriteLine(data.id +" send");
        await Clients.Group(data.id).SendAsync("Receive", data.message);
       // await Clients.Group(data.id).SendAsync("ReceiveMessage", data.message);
    }

    public async Task JoinChatGroup(string chatId)
    {
        Console.WriteLine("group"+chatId);
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
    }

    public async Task LeaveChatGroup(string chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
    }
}

public class Data
{
    public string id { get; set; }
    public string message { get; set; }
}
