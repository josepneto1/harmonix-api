using FluentValidation;

namespace Harmonix.Api.Features.Staff.Companies.Create;

public class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest>
{
    public CreateCompanyRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome da empresa é obrigatório")
            .MinimumLength(3).WithMessage("O nome não pode ter menos de 3 caracteres")
            .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres");
        RuleFor(x => x.Alias)
            .NotEmpty().WithMessage("O alias é obrigatório")
            .MinimumLength(3).WithMessage("O alias não pode ter menos de 3 caracteres")
            .MaximumLength(30).WithMessage("O alias não pode ter mais de 30 caracteres");
        RuleFor(x => x.ExpirationDate)
            .Must(e => e > DateTimeOffset.UtcNow).WithMessage("A data de expiração deve ser futura");
    }
}
