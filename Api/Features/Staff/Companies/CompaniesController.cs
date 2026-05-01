using Harmonix.Api.Features.Staff.Companies.Activate;
using Harmonix.Api.Features.Staff.Companies.Create;
using Harmonix.Api.Features.Staff.Companies.Delete;
using Harmonix.Api.Features.Staff.Companies.Get;
using Harmonix.Api.Features.Staff.Companies.List;
using Harmonix.Api.Features.Staff.Companies.Update;
using Harmonix.Common;
using Harmonix.Domain.Users.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Harmonix.Api.Features.Staff.Companies;

[ApiController]
[Route("api/staff/[controller]")]
[Authorize(Roles = nameof(Role.SysAdmin))]
public class CompaniesController : ControllerBase
{
    [HttpGet("company/{id:guid}")]
    public async Task<IActionResult> GetCompanyById(Guid id, GetCompanyByIdHandler handler, CancellationToken ct)
    {
        var result = await handler.ExecuteAsync(id, ct);
        return this.GetResult(result);
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListCompanies(
        [FromQuery] ListCompaniesRequest request,
        ListCompaniesHandler handler,
        CancellationToken ct)
    {
        var result = await handler.ExecuteAsync(request, ct);
        return this.GetResult(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequest request, CreateCompanyHandler handler)
    {
        var result = await handler.ExecuteAsync(request);
        return this.GetResult(result);
    }

    [HttpPatch("update")]
    public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyRequest request, UpdateCompanyHandler handler)
    {
        var result = await handler.ExecuteAsync(request);
        return this.GetResult(result);
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid id, DeleteCompanyHandler handler)
    {
        var result = await handler.ExecuteAsync(id);
        return this.GetResult(result);
    }

    [HttpPatch("changeStatus")]
    public async Task<IActionResult> SetCompanyStatus([FromBody] SetCompanyStatusRequest request, SetCompanyStatusHandler handler)
    {
        var result = await handler.ExecuteAsync(request);
        return this.GetResult(result);
    }
}
