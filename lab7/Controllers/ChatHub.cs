using lab7.Data;
using lab7.Models;
using Microsoft.AspNetCore.SignalR;

namespace lab7.Controllers;

public class ChatHub : Hub
{
    private readonly AppDbContext dbContext;

    public ChatHub(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task SendMessage(Message message)
    {
        message.Timestamp = DateTime.UtcNow;
        // Сохранить сообщение в базе данных
        dbContext.Messages.Add(message);
        await dbContext.SaveChangesAsync();

        // Отправить сообщение получателю
        await Clients.User(message.ReceiverId).SendAsync("ReceiveMessage", message);

        // Отправить сообщение отправителю
        await Clients.User(message.SenderId).SendAsync("ReceiveMessage", message);
    }
    
    
}
