using Harmonix.Domain.Auth;
using Harmonix.Domain.Common;
using Harmonix.Domain.Common.Errors;
using Harmonix.Domain.Common.Validations;
using Harmonix.Domain.Common.ValueObjects;
using Harmonix.Domain.Companies;
using Harmonix.Domain.Users.Enums;

namespace Harmonix.Domain.Users;

public class User : BaseEntity
{
    public Guid CompanyId { get; private set; }
    public string Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public Role Role { get; private set; }
    public Company Company { get; private set; } = null!;
    public List<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();

    protected User() { }

    private User(
        Guid companyId,
        string name,
        Email email,
        Role role)
    {
        Id = GenerateNewId();
        CompanyId = companyId;
        Name = name;
        Email = email;
        Role = role;
    }

    public static Result<User> Create(Guid companyId, string name, string email, Role role)
    {
        name = name.Trim();

        if (!Validate.IsValidText(name, 3, 100))
            return Result<User>.Fail(CommonErrors.InvalidName);

        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
            return Result<User>.Fail(emailResult.Error);

        if (!Enum.IsDefined(role))
            return Result<User>.Fail(CommonErrors.InvalidRole);

        var user = new User(companyId, name, emailResult.Data, role);
        return Result<User>.Success(user);
    }

    public void Update(string? name, string? email, Role? role)
    {
        if (name is not null)
            Name = name.Trim();

        if (email is not null)
            Email = Email.Create(email).Data!;

        if (role is Role r)
            Role = r;
    }

    public void SetPasswordHash(string hash) => PasswordHash = hash;

    public Result CanAuthenticate()
    {
        if (Removed || Company.Removed || !Company.IsActive || Company.ExpirationDate <= DateTimeOffset.UtcNow)
            return Result.Fail(AuthErrors.InvalidCredentials);

        return Result.Success();
    }
}
