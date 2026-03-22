    using FluentValidation;
    using Harmonix.Application.Common;
    using Harmonix.Application.Common.Errors;
    using Harmonix.Application.Common.Results;
using Harmonix.Domain.Companies;
using Harmonix.Domain.Companies.Services;
using Harmonix.Domain.Companies.ValueObjects;
using Harmonix.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    namespace Harmonix.Api.Features.Staff.Companies.Update;

    public class UpdateCompanyHandler : BaseHandler<UpdateCompanyRequest, UpdateCompanyResponse>
    {
        private readonly HarmonixDbContext _context;
        private readonly IAliasUniqueChecker _aliasChecker;

        public UpdateCompanyHandler(
            HarmonixDbContext context,
            IValidator<UpdateCompanyRequest> validator,
            IAliasUniqueChecker aliasChecker) 
            : base(validator)
        {
            _context = context;
            _aliasChecker = aliasChecker;
        }

        protected override async Task<Result<UpdateCompanyResponse>> HandleAsync(UpdateCompanyRequest request,CancellationToken ct)
        {
            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.Id == request.Id && !c.Removed);

            if (company is null)
                return Result<UpdateCompanyResponse>.Fail(CommonError.NotFound);

            Alias? alias = null;

            if (request.Alias is not null)
            {
                var aliasResult = Alias.Create(request.Alias);
                if (aliasResult.IsFailure)
                    return Result<UpdateCompanyResponse>.Fail(aliasResult.Error);

                alias = aliasResult.Value!;

                if (alias != company.Alias)
                {
                    var isUnique = await _aliasChecker.IsUniqueAsync(alias);
                    if (!isUnique)
                        return Result<UpdateCompanyResponse>.Fail(CompanyErrors.AliasAlreadyExists);
                }
            }

            var updateResult = company.Update(request.Name, alias, request.ExpirationDate);
            if (updateResult.IsFailure)
                return Result<UpdateCompanyResponse>.Fail(updateResult.Error);

            await _context.SaveChangesAsync();

            var response = new UpdateCompanyResponse(company.Id, company.Name);

            return Result<UpdateCompanyResponse>.Success(response);
        }
    }
