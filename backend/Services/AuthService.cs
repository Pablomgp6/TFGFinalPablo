using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;

public class AuthService
{
    private readonly IMongoCollection<User> _users;

    public AuthService(IMongoClient client)
    {
        var db = client.GetDatabase("Pokemon");
        _users = db.GetCollection<User>("Users");
    }

    public async Task<bool> EmailExists(string email)
    {
        return await _users.Find(u => u.Email == email).AnyAsync();
    }

    public async Task<bool> Register(string email, string username, string password)
    {
        if (await EmailExists(email)) return false;

        var user = new User
        {
            Email = Sanitize(email),
            Username = Sanitize(username),
            PasswordHash = Hash(password)
        };

        await _users.InsertOneAsync(user);
        return true;
    }

    public async Task<User?> Login(string email, string password)
    {
        var hash = Hash(password);
        return await _users.Find(u => u.Email == email && u.PasswordHash == hash).FirstOrDefaultAsync();
    }

    private string Hash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hashBytes);
    }

    private string Sanitize(string input)
    {
        return input.Trim().Replace("<", "").Replace(">", "").Replace("\"", "").Replace("'", "");
    }
}
