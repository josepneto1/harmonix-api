using FluentValidation;
using Harmonix.Application.Common;
using Harmonix.Domain.Common;
using Harmonix.Domain.Companies;
using Harmonix.Domain.Companies.Services;
using Harmonix.Infrastructure.Data;

namespace Harmonix.Api.Features.Staff.Companies.Create;

public class CreateCompanyHandler : BaseHandler<CreateCompanyRequest, CreateCompanyResponse>
{
    private readonly HarmonixDbContext _context;
    private readonly IAliasUniqueChecker _aliasChecker;

    public CreateCompanyHandler(
        HarmonixDbContext context,
        IAliasUniqueChecker aliasChecker,
        IValidator<CreateCompanyRequest> validator)
        : base (validator)
    {
        _context = context;
        _aliasChecker = aliasChecker;
    }

    protected override async Task<Result<CreateCompanyResponse>> HandleAsync(CreateCompanyRequest request, CancellationToken ct)
    {
        var companyResult = Company.Create(
            request.Name,
            request.Alias,
            request.ExpirationDate
        );

        if (companyResult.IsFailure)
            return Result<CreateCompanyResponse>.Fail(companyResult.Error);

        var company = companyResult.Data!;

        var isUnique = await _aliasChecker.IsUniqueAsync(company.Alias);
        if (!isUnique)
            return Result<CreateCompanyResponse>.Fail(CompanyErrors.AliasAlreadyExists);

        _context.Companies.Add(company);

        await _context.SaveChangesAsync();

        var response = new CreateCompanyResponse(
            company.Id,
            company.Name,
            company.Alias.Value,
            company.CreatedAt,
            company.ExpirationDate
        );

        return Result<CreateCompanyResponse>.Success(response);
    }
}
