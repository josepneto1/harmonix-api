using Harmonix.Application.Common;
using Harmonix.Application.Common.Errors;
using Harmonix.Application.Common.Results;
using Harmonix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Harmonix.Api.Features.Auth.Logout;

public class LogoutHandler : BaseHandler<LogoutRequest, LogoutResponse>
{
    private readonly HarmonixDbContext _context;

    public LogoutHandler(HarmonixDbContext context)
    {
        _context = context;
    }

    protected override async Task<Result<LogoutResponse>> HandleAsync(LogoutRequest request, CancellationToken ct)
    {
        var refreshToken = await _context.RefreshTokens
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken, ct);

        if (refreshToken is null)
            return Result<LogoutResponse>.Fail(CommonError.InternalError);

        if (refreshToken.IsRevoked)
            return Result<LogoutResponse>.Success(new LogoutResponse(true, "Token already revoked"));

        refreshToken.Revoke();
        await _context.SaveChangesAsync(ct);

        return Result<LogoutResponse>.Success(new LogoutResponse(true, "Logout successful"));
    }
}

public record LogoutRequest(string RefreshToken);

public record LogoutResponse(bool Success, string Message);