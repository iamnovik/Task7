using Microsoft.AspNetCore.Mvc;

namespace lab7.Controllers;

public class VideoCallController : Controller
{
    public IActionResult Index(string chatId, string senderId)
    {
        ViewBag.ChatId = chatId;
        ViewBag.SenderId = senderId;
        return View();
    }


}