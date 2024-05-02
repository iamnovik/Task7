using Microsoft.AspNetCore.Mvc;

namespace lab7.Controllers;

public class VideoCallController : Controller
{
    [HttpGet("{chatId}")]
    public IActionResult Index(string chatId)
    {
        ViewBag.ChatId = chatId;
        return View();
    }


}