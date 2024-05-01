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
        var messages = await dbContext.Messages
            .Where(m => (m.SenderId == user.Id && m.ReceiverId == contactId) || (m.SenderId == contactId && m.ReceiverId == user.Id))
            .OrderBy(m => m.Timestamp)
            .ToListAsync();

        ViewBag.CurrentUserId = user.Id;
        ViewBag.ContactId = contactId;
        var receiver = await userManager.FindByIdAsync(contactId);
        ViewBag.ContactName = receiver.FullName;
        return View(messages);
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