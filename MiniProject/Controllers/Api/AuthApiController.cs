using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniProject.Common.ViewModel;
using MiniProject.Services.Auth;

namespace MiniProject.Controllers.Api;

[ApiController]
[Route("api/auth")]
public class AuthApiController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthApiController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model, CancellationToken cancellationToken)
    {
        var user = await _authService.ValidateAsync(model.Username, model.Password, cancellationToken);
        if (user == null)
        {
            return BadRequest(new { message = "Invalid username or password." });
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.TxtUsername),
            new Claim(ClaimTypes.NameIdentifier, user.Uid.ToString()),
            new Claim("FullName", user.TxtFullName)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        return Ok(new { username = user.TxtUsername, fullName = user.TxtFullName });
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new { message = "Logged out." });
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        return Ok(new { username = User.Identity?.Name, fullName = User.FindFirst("FullName")?.Value });
    }
}
