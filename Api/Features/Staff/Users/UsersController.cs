using Harmonix.Api.Features.Staff.Users.Create;
using Harmonix.Api.Features.Staff.Users.Delete;
using Harmonix.Api.Features.Staff.Users.Get;
using Harmonix.Api.Features.Staff.Users.List;
using Harmonix.Api.Features.Staff.Users.Update;
using Harmonix.Common;
using Harmonix.Domain.Users.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Harmonix.Api.Features.Staff.Users;

[ApiController]
[Route("api/staff/[controller]")]
[Authorize(Roles = nameof(Role.SysAdmin))]
public class UsersController : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserRequest request,
        CreateUserHandler handler,
        CancellationToken ct)
    {
        var result = await handler.ExecuteAsync(request, ct);
        return this.GetResult(result);
    }

    [HttpGet("user/{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id, GetUserByIdHandler handler, CancellationToken ct)
    {
        var result = await handler.ExecuteAsync(id, ct);
        return this.GetResult(result);
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListUsers(ListUsersHandler handler, CancellationToken ct)
    {
        var result = await handler.ExecuteAsync(ct);
        return this.GetResult(result);
    }

    [HttpPatch("update")]
    public async Task<IActionResult> UpdateUser(
        [FromBody] UpdateUserRequest request,
        UpdateUserHandler handler,
        CancellationToken ct)
    {
        var result = await handler.ExecuteAsync(request, ct);
        return this.GetResult(result);
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid id, DeleteUserHandler handler, CancellationToken ct)
    {
        var result = await handler.ExecuteAsync(id, ct);
        return this.GetResult(result);
    }
}
