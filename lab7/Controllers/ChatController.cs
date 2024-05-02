using lab7.Data;
using lab7.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab7.Controllers;

public class ChatController : Controller
{
    private readonly AppDbContext dbContext;
    private readonly UserManager<Contact> userManager;

    public ChatController(AppDbContext dbContext, UserManager<Contact> userManager)
    {
        this.dbContext = dbContext;
        this.userManager = userManager;
    }

    public async Task<IActionResult> Index(string contactId)
    {
        var user = await userManager.GetUserAsync(User);
        var contact = await userManager.FindByIdAsync(contactId);
            
        if (contact == null)
            return NotFound("Contact not found.");

        // Проверяем, существует ли уже чат между текущим пользователем и контактом
        var existingChat = await dbContext.Chats
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c =>(c.SenderId == user.Id  && c.ReceiverId == contactId)|| (c.SenderId == contactId && c.ReceiverId == user.Id));

        if (existingChat == null)
        {
            // Если чата между пользователями нет, создаем новый чат
            var newChat = new Chat
            {
                SenderId = user.Id,
                ReceiverId = contactId,
            };

            dbContext.Chats.Add(newChat);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { contactId });
        }

        ViewBag.CurrentUserId = user.Id;
        ViewBag.ContactId = contactId;
        ViewBag.ChatId = existingChat.Id;
        ViewBag.ContactName = contact.FullName;
        return View(existingChat.Messages.ToList());
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateMessage(int messageId, string newText)
    {
        var message = await dbContext.Messages.FirstOrDefaultAsync(m => m.Id == messageId);
        if (message == null)
        {
            return NotFound();
        }

        message.Text = newText;
        await dbContext.SaveChangesAsync();

        return Ok();
    }

}