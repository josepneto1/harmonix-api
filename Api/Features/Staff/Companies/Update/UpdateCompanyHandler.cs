    using FluentValidation;
    using Harmonix.Application.Common;
    using Harmonix.Application.Common.Errors;
    using Harmonix.Application.Common.Results;
    using Harmonix.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    namespace Harmonix.Api.Features.Staff.Companies.Update;

    public class UpdateCompanyHandler : BaseHandler<UpdateCompanyRequest, UpdateCompanyResponse>
    {
        private readonly HarmonixDbContext _context;

        public UpdateCompanyHandler(
            HarmonixDbContext context,
            IValidator<UpdateCompanyRequest> validator) 
            : base(validator)
        {
            _context = context;
        }

        protected override async Task<Result<UpdateCompanyResponse>> HandleAsync(UpdateCompanyRequest request,CancellationToken ct)
        {
            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.Id == request.Id && !c.Removed, ct);

            if (company is null)
                return Result<UpdateCompanyResponse>.Fail(CommonError.NotFound);

            var updateResult = company.Update(request.Name, request.Alias, request.ExpirationDate);
            if (updateResult.IsFailure)
                return Result<UpdateCompanyResponse>.Fail(updateResult.Error);

            await _context.SaveChangesAsync(ct);

            var response = new UpdateCompanyResponse(company.Id, company.Name);

            return Result<UpdateCompanyResponse>.Success(response);
        }
    }
