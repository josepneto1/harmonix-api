using FluentValidation;
using Harmonix.Application.Common;
using Harmonix.Domain.Auth;
using Harmonix.Domain.Common;
using Harmonix.Domain.Common.Errors;
using Harmonix.Domain.Common.ValueObjects;
using Harmonix.Infrastructure.Auth;
using Harmonix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Harmonix.Api.Features.Auth.Login; 

public class LoginHandler : BaseHandler<LoginRequest, LoginResponse>
{ 
    private readonly HarmonixDbContext _context;
    private readonly JwtTokenProvider _jwtTokenProvider;
    private readonly IPasswordHasher _passwordHasher;
    private readonly JwtSettings _jwtSettings;

    public LoginHandler(
        HarmonixDbContext context,
        JwtTokenProvider jwtTokenProvider,
        IPasswordHasher passwordHasher,
        JwtSettings jwtSettings)
    {
        _context = context;
        _jwtTokenProvider = jwtTokenProvider;
        _passwordHasher = passwordHasher;
        _jwtSettings = jwtSettings;
    }

    protected override async Task<Result<LoginResponse>> HandleAsync(LoginRequest request, CancellationToken ct)
    {
        var userEmailResult = Email.Create(request.Email);

        if (userEmailResult.IsFailure)
            return Result<LoginResponse>.Fail(userEmailResult.Error);

        var user = await _context.Users
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Include(u => u.Company)
            .FirstOrDefaultAsync(u => u.Email == userEmailResult.Data, ct);

        var passwordHash = user?.PasswordHash ?? _passwordHasher.FakeHash;

        var passwordValid = _passwordHasher.VerifyPassword(request.Password, passwordHash);

        if (user is null || !passwordValid || !user.Company.IsActive)
            return Result<LoginResponse>.Fail(AuthErrors.InvalidCredentials);

        var activeRefreshTokens = await _context.RefreshTokens
            .Where(rt =>
                rt.UserId == user.Id &&
                rt.RevokedAt == null &&
                rt.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(ct);

        foreach (var token in activeRefreshTokens)
        {
            token.Revoke();
        }

        var (accessToken, accessExpiresAt) = _jwtTokenProvider.GenerateToken(user);

        var refreshTokenString = _jwtTokenProvider.GenerateRefreshToken();
        var refreshExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
        var refreshToken = new RefreshToken(user.Id, user.CompanyId, refreshTokenString, refreshExpiresAt);

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync(ct);

        var response = new LoginResponse(
            accessToken,
            accessExpiresAt,
            refreshTokenString,
            refreshExpiresAt
        );

        return Result<LoginResponse>.Success(response);
    }
}

public record LoginRequest(string Email, string Password);

public record LoginResponse(
    string AccessToken,
    DateTime AccessExpiresAt,
    string RefreshToken,
    DateTime RefreshExpiresAt
);
