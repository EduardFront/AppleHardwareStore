using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AppleHardwareStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AppleHardwareStore.Models;
using AppleHardwareStore.Services;

namespace AppleHardwareStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppleHardwareStoreDbContext _dbContext;
        private readonly IMessageSender _messageSender;
        public HomeController(ILogger<HomeController> logger, AppleHardwareStoreDbContext dbContext, IMessageSender messageSender)
        {
            _logger = logger;
            _dbContext = dbContext;
            _messageSender = messageSender;
        }

        public IActionResult Index()
        {
            ViewBag.Message = _messageSender.Send("Сообщение");
            return View(_dbContext.ProductTypes.OrderBy(item => item.Name).ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}