using CarritoDeComprasMVC.Service;
using Microsoft.AspNetCore.Mvc;

namespace CarritoDeComprasMVC.Controllers;

public class AccesController : Controller
{
    private readonly UserService _userService;

    public AccesController(UserService userService)
    {
        _userService = userService;
    }
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var isValidUser = await _userService.VerifyPasswordAsync(email, password);

        if (isValidUser)
        {
            return RedirectToAction("Index", "Tienda");
        }

        // Autenticación fallida: muestra un mensaje de error
        ViewBag.ErrorMessage = "Correo electrónico o contraseña incorrectos.";
        return View();
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(string name, string email, string password)
    {
        await _userService.CreateUserAsync(name, email, password);

        return RedirectToAction("Login");
    }
}