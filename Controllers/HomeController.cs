using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TiendaEcomerce.Models;

namespace TiendaEcomerce.Controllers;

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

    [HttpGet]
    public IActionResult Test()
    {
        ViewBag.Variable = "Hola Bienvenido a MVC NetCore";
        return View();
    }

    [HttpGet]
    public IActionResult Get()
    {
        var  objeto= new {Nombre="Erik",ApellidoPaterno="Guerrero",ApellidoMaterno="Bravo" };

        return Json(objeto);
    }
}
