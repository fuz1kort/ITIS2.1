using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Temp.Extensions;
using Temp.Models;

namespace Temp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Calculator()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Calculate(string first, string operation, string second)
    {
        var result = CalculatorExtension.Calculate(first, operation, second);

        return View(result);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}