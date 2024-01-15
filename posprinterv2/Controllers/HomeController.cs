using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using Microsoft.AspNetCore.Mvc;
using posprinterv2.Models;
using System.Diagnostics;
using Newtonsoft.Json;


namespace POSWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static BasePrinter printer;
        private static ICommandEmitter e;


        private static string ip = "192.168.1.226";
        private static string networkPort = "9100";
        private static PrinterStatusEventArgs status = new PrinterStatusEventArgs();


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
            Setup(true);

            printer.Write(Tests.SingleLinePrinting(e));


            return Ok(status.IsPrinterOnline);
        }
        private static void StatusChanged(object sender, EventArgs ps)
        {
            status = (PrinterStatusEventArgs)ps;
            if (status == null) { Console.WriteLine("Status was null - unable to read status from printer."); return; }
            Console.WriteLine($"Printer Online Status: {status.IsPrinterOnline}");
            Console.WriteLine(JsonConvert.SerializeObject(status));
        }
        private static bool _hasEnabledStatusMonitoring = false;

        private static void Setup(bool enableStatusBackMonitoring)
        {
            if (printer != null)
            {
                // Only register status monitoring once.
                if (!_hasEnabledStatusMonitoring)
                {
                    printer.StatusChanged += StatusChanged;
                    _hasEnabledStatusMonitoring = true;
                }
                printer?.Write(e.Initialize());
                printer?.Write(e.Enable());
                if (enableStatusBackMonitoring)
                {
                    printer.Write(e.EnableAutomaticStatusBack());
                }
            }
        }
    }
}
