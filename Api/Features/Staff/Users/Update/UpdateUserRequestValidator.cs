using FluentValidation;

namespace Harmonix.Api.Features.Staff.Users.Update;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório")
            .MinimumLength(3).WithMessage("O nome não pode ter menos de 3 caracteres")
            .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres")
            .When(x => x.Name is not null);
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório")
            .EmailAddress().WithMessage("O email informado é inválido")
            .MaximumLength(255).WithMessage("O email não pode ter mais de 255 caracteres")
            .When(x => x.Email is not null);
        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("O papel informado é inválido")
            .When(x => x.Role is not null);
    }
}