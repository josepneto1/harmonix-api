using Harmonix.Application.Common;
using Harmonix.Domain.Common.Errors;
using Harmonix.Domain.Auth;
using Harmonix.Domain.Common;
using Harmonix.Infrastructure.Auth;
using Harmonix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Harmonix.Api.Features.Auth.Refresh;

public class RefreshTokenHandler : BaseHandler<RefreshTokenRequest, RefreshTokenResponse>
{
    private readonly HarmonixDbContext _context;
    private readonly JwtTokenProvider _jwtTokenProvider;
    private readonly JwtSettings _jwtSettings;

    public RefreshTokenHandler(
        HarmonixDbContext context,
        JwtTokenProvider jwtTokenProvider,
        JwtSettings jwtSettings)
    {
        _context = context;
        _jwtTokenProvider = jwtTokenProvider;
        _jwtSettings = jwtSettings;
    }

    protected override async Task<Result<RefreshTokenResponse>> HandleAsync(RefreshTokenRequest request, CancellationToken ct)
    {
        var refreshToken = await _context.RefreshTokens
            .Where(rt => rt.Token == request.RefreshToken)
            .Include(rt => rt.User)
                .ThenInclude(u => u.Company)
            .FirstOrDefaultAsync();

        if (refreshToken is null || !refreshToken.IsValid)
            return Result<RefreshTokenResponse>.Fail(AuthErrors.InvalidRefreshToken);

        refreshToken.Revoke();

        var user = refreshToken.User;

        if (user is null || !user.Company.IsActive)
            return Result<RefreshTokenResponse>.Fail(AuthErrors.InvalidRefreshToken);

        var (newAccessToken, accessExpiresAt) = _jwtTokenProvider.GenerateToken(user);

        var newRefreshTokenString = _jwtTokenProvider.GenerateRefreshToken();
        var newRefreshExpiresAt = DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

        var newRefreshToken = new RefreshToken(user.Id, user.CompanyId, newRefreshTokenString, newRefreshExpiresAt);

        _context.RefreshTokens.Add(newRefreshToken);

        await _context.SaveChangesAsync();

        var response = new RefreshTokenResponse(
            newAccessToken,
            newRefreshTokenString,
            accessExpiresAt,
            newRefreshExpiresAt
        );

        return Result<RefreshTokenResponse>.Success(response);
    }
}

public record RefreshTokenRequest(string RefreshToken);

public record RefreshTokenResponse(
    string AccessToken,
    string RefreshToken,
    DateTimeOffset AccessExpiresAt,
    DateTimeOffset RefreshExpiresAt
);
