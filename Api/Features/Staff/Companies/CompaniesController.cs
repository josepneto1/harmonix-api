using Harmonix.Api.Common.Extensions;
using Harmonix.Api.Features.Staff.Companies.Activate;
using Harmonix.Api.Features.Staff.Companies.Create;
using Harmonix.Api.Features.Staff.Companies.Delete;
using Harmonix.Api.Features.Staff.Companies.Get;
using Harmonix.Api.Features.Staff.Companies.List;
using Harmonix.Api.Features.Staff.Companies.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Harmonix.Api.Features.Staff.Companies;

[ApiController]
[Route("api/staff/[controller]")]
[Authorize(Roles = "SysAdmin")]
public class CompaniesController : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateCompany(
        [FromBody] CreateCompanyRequest request, 
        CreateCompanyHandler handler,
        CancellationToken ct)
    {
        var result = await handler.ExecuteAsync(request, ct);
        return this.GetResult(result);
    }

    [HttpGet("company/{id:guid}")]
    public async Task<IActionResult> GetCompanyById(Guid id, GetCompanyByIdHandler handler, CancellationToken ct)
    {
        var result = await handler.ExecuteAsync(id, ct);
        return this.GetResult(result);
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListCompanies(ListCompaniesHandler handler, CancellationToken ct)
    {
        var result = await handler.ExecuteAsync(ct);
        return this.GetResult(result);
    }

    [HttpPatch("update")]
    public async Task<IActionResult> UpdateCompany(
        [FromBody] UpdateCompanyRequest request,
        UpdateCompanyHandler handler,
        CancellationToken ct)
    {
        var result = await handler.ExecuteAsync(request, ct);
        return this.GetResult(result);
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid id, DeleteCompanyHandler handler,CancellationToken ct)
    {
        var result = await handler.ExecuteAsync(id, ct);
        return this.GetResult(result);
    }

    [HttpPut("changeStatus")]
    public async Task<IActionResult> SetCompanyStatus(
        [FromBody] SetCompanyStatusRequest request,
        SetCompanyStatusHandler handler,
        CancellationToken ct)
    {
        var result = await handler.ExecuteAsync(request, ct);
        return this.GetResult(result);
    }
}
