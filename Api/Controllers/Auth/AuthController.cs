using Financer.Infrastructure.Authentication;
using Financer.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Financer.Api.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;

    public AuthController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ITokenService tokenService
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ProblemDetails { Detail = "Invalid request data." });
        }

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Unauthorized(new ProblemDetails { Detail = "Invalid email or password." });
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            return Unauthorized(new ProblemDetails { Detail = "Invalid email or password." });
        }

        var (accessToken, refreshToken) = await _tokenService.GenerateTokensAsync(user);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7),
        };

        Response.Cookies.Append("access_token", accessToken, cookieOptions);
        Response.Cookies.Append("refresh_token", refreshToken, cookieOptions);

        // Also return tokens in JSON response
        return Ok(new LoginResponse { AccessToken = accessToken, RefreshToken = refreshToken });
    }
}
