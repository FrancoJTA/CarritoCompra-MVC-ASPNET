using CarritoDeComprasMVC.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace CarritoDeComprasMVC.Service;

public class UserService
{
    private readonly IMongoCollection<User> _users;
    private readonly PasswordHasher<User> _passwordHasher;

    public UserService(IMongoDatabase database)
    {
        _users = database.GetCollection<User>("Users");
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task CreateUserAsync(string name, string email, string plainPassword)
    {
        // Crear el objeto de usuario
        var user = new User
        {
            Name = name,
            Email = email
        };

        // Encriptar la contraseña y guardarla en el campo PasswordHash
        user.PasswordHash = _passwordHasher.HashPassword(user, plainPassword);

        // Insertar el usuario en la colección
        await _users.InsertOneAsync(user);
    }

    public async Task<bool> VerifyPasswordAsync(string email, string plainPassword)
    {
        // Obtener el usuario por email
        var user = await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
        if (user == null)
            return false;
        
        // Verificar la contraseña en texto plano contra el hash almacenado
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, plainPassword);

        return result == PasswordVerificationResult.Success;
    }


    public async Task<List<User>> GetAllUsersAsync() =>
        await _users.Find(_ => true).ToListAsync();

    public async Task<User?> GetUserByIdAsync(Guid id) =>
        await _users.Find(user => user.Id == id.ToString()).FirstOrDefaultAsync();

    public async Task CreateUserAsync(User user) =>
        await _users.InsertOneAsync(user);

    public async Task UpdateUserAsync(Guid id, User updatedUser) =>
        await _users.ReplaceOneAsync(user => user.Id == id.ToString(), updatedUser);

    public async Task DeleteUserAsync(Guid id) =>
        await _users.DeleteOneAsync(user => user.Id == id.ToString());
}