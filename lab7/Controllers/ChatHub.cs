using lab7.Data;
using lab7.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

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
        Chat chat = await dbContext.Chats.FirstOrDefaultAsync(c => c.Id == message.ChatId);
        message.Chat = chat;
        // Сохранить сообщение в базе данных
        dbContext.Messages.Add(message);
        await dbContext.SaveChangesAsync();
        var receiverId = chat.ReceiverId;
        message.Chat = null;
        // Отправить сообщение получателю
        await Clients.User(chat.ReceiverId).SendAsync("ReceiveMessage",message);

        // Отправить сообщение отправителю
        await Clients.User(chat.SenderId).SendAsync("ReceiveMessage",message);
    }
    
    
}
