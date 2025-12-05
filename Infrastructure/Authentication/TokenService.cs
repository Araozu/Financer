using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Financer.Infrastructure.Configuration;
using Financer.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Financer.Infrastructure.Authentication;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<User> _userManager;
    private readonly Dictionary<string, (string UserId, DateTime Expiration)> _refreshTokenStore =
        new();

    public TokenService(IOptions<JwtSettings> jwtSettings, UserManager<User> userManager)
    {
        _jwtSettings = jwtSettings.Value;
        _userManager = userManager;
    }

    public async Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(User user)
    {
        var accessToken = await GenerateAccessTokenAsync(user);
        var refreshToken = GenerateRefreshToken();

        // Store refresh token with user ID and expiration
        var expiration = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
        _refreshTokenStore[refreshToken] = (user.Id, expiration);

        return (accessToken, refreshToken);
    }

    public Task<string?> ValidateRefreshTokenAsync(string refreshToken)
    {
        if (_refreshTokenStore.TryGetValue(refreshToken, out var tokenData))
        {
            if (tokenData.Expiration > DateTime.UtcNow)
            {
                return Task.FromResult<string?>(tokenData.UserId);
            }
            // Token expired, remove it
            _refreshTokenStore.Remove(refreshToken);
        }
        return Task.FromResult<string?>(null);
    }

    private async Task<string> GenerateAccessTokenAsync(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
