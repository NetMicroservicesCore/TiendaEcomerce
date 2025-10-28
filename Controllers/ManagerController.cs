using Dominio;
using Microsoft.AspNetCore.Mvc;

namespace TiendaEcomerce.Controllers
{
    public class ManagerController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index([Bind(include: "Digito1,Digito2")]Operaciones model)
        {
            model.Resultado = model.Digito1 + model.Digito2;
            return View(model);
        }


        public IActionResult Registro()
        {
            return View();
        }


    }
}
