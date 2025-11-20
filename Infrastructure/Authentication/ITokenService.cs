using Financer.Infrastructure.Entities;

namespace Financer.Infrastructure.Authentication;

public interface ITokenService
{
    Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(User user);
    Task<string?> ValidateRefreshTokenAsync(string refreshToken);
}
