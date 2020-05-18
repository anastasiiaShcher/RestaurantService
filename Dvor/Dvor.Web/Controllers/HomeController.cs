using Dvor.Common.Entities;
using Dvor.Common.Enums;
using Dvor.Common.Interfaces;
using Dvor.Common.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dvor.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDishService _dishService;
        private readonly IMailService _mailService;

        public HomeController(IDishService dishService, IMailService mailService)
        {
            _dishService = dishService;
            _mailService = mailService;
        }

        public IActionResult Index()
        {
            var categories = _dishService.GetCategories();

            return View(categories);
        }

        [HttpPost]
        public IActionResult CallWaiter()
        {
            var mailContent = $"Please go to the table 2T";

            var notification = new Notification
            {
                Title = "Waiter call",
                Content = mailContent
            };

            _mailService.Send("sher210400@gmail.com", notification);

            return Ok();
        }
    }
}