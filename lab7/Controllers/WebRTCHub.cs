using Microsoft.AspNetCore.SignalR;

namespace lab7.Controllers;

public class WebRTCHub : Hub
{
    public async Task Send(string message)
    {
        await Clients.Others.SendAsync("Receive", message);
    }
}