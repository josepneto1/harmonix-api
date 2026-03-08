using FluentValidation;

namespace Harmonix.Api.Features.Staff.Users.Create;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty().WithMessage("A empresa é obrigatória");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório")
            .MinimumLength(3).WithMessage("O nome não pode ter menos de 3 caracteres")
            .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório")
            .EmailAddress().WithMessage("O email informado é inválido")
            .MaximumLength(255).WithMessage("O email não pode ter mais de 255 caracteres");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória")
            .MinimumLength(8).WithMessage("A senha não pode ter menos de 8 caracteres")
            .MaximumLength(20).WithMessage("A senha não pode ter mais de 20 caracteres");
        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("O papel informado é inválido");
    }
}
