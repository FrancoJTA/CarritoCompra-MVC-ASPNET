using CarritoDeComprasMVC.Data;
using CarritoDeComprasMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarritoDeComprasMVC.Controllers;

public class AccesController : Controller
{
    private readonly MyDBContext _context;
    private readonly PasswordHasher<User> _passwordHasher;

    public AccesController(MyDBContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<User>();
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
        // Buscar el usuario en la base de datos por su correo electrónico
        var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            // Usuario no encontrado
            ViewBag.ErrorMessage = "Correo electrónico o contraseña incorrectos.";
            return View();
        }

        // Verificar la contraseña en texto plano contra el hash almacenado
        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

        if (result == PasswordVerificationResult.Success)
        {
            // Autenticación exitosa, redirigir al controlador Tienda, acción Index
            return RedirectToAction("Index", "Tienda");
        }
        else
        {
            // Contraseña incorrecta
            ViewBag.ErrorMessage = "Correo electrónico o contraseña incorrectos.";
            return View();
        }
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(string name, string email, string password)
    {
        // Crear el usuario y encriptar la contraseña
        var user = new User
        {
            Name = name,
            Email = email,
            Password = _passwordHasher.HashPassword(null, password) // Encriptar la contraseña
        };

        // Guardar el usuario en la base de datos
        _context.Usuarios.Add(user);
        await _context.SaveChangesAsync();

        // Redirigir a la página de Login después del registro exitoso
        return RedirectToAction("Login");
    }
}