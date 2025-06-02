using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _auth;

    public AuthController(AuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("register")]
public async Task<IActionResult> Register([FromBody] RegisterRequest req)
{
    if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
    {
        return BadRequest(new { error = "Faltan campos requeridos." });
    }

    var success = await _auth.Register(req.Email, req.Username, req.Password);

    if (success)
    {
        // ✅ Devuelve email y username para guardarlos en localStorage
        return Ok(new { username = req.Username, email = req.Email });
    }
    else
    {
        return Conflict(new { error = "El email ya está registrado." });
    }
}


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
        {
            return BadRequest(new { error = "Faltan campos requeridos." });
        }

        var user = await _auth.Login(req.Email, req.Password);

        if (user == null)
        {
            return Unauthorized(new { error = "Credenciales incorrectas." });
        }

        return Ok(new { user.Username, user.Email });
    }
}

public class RegisterRequest
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
