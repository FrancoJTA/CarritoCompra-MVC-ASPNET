using Microsoft.AspNetCore.Mvc;

namespace CarritoDeComprasMVC.Controllers;

public class TiendaController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}