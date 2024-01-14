using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using Microsoft.AspNetCore.Mvc;
using posprinterv2.Models;
using System.Diagnostics;

namespace POSWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static BasePrinter printer;
        private static ICommandEmitter e;


        private static string ip = "192.168.1.226";
        private static string networkPort = "9100";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Print()
        {
            printer = new NetworkPrinter(settings: new NetworkPrinterSettings() { ConnectionString = $"{ip}:{networkPort}" });
            e = new EPSON();
            printer.Write(Tests.SingleLinePrinting(e));

            return Ok();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
