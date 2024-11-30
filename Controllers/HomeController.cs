using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using amazonBooks.Models;

namespace amazonBooks.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
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

    public IActionResult ViewPdf(string filePath)
    {
        var allowedDirectories = new[] { @"C:\MyPDFs", @"D:\MorePDFs" };
        if (!allowedDirectories.Any(dir => filePath.StartsWith(dir)) || !System.IO.File.Exists(filePath))
        {
            return NotFound("File not found.");
        }

        return File(System.IO.File.ReadAllBytes(filePath), "application/pdf");
    }

}
