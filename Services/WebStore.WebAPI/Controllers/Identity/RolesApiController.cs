using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GB.ASPNET.WebStore.DAL.Context;
using GB.ASPNET.WebStore.Domain.Entities.Identity;
using GB.ASPNET.WebStore.Interfaces;

namespace GB.ASPNET.WebStore.WebAPI.Controllers;

[Route(WebApiRoutes.V1.IdentityRolesRoute)]
[ApiController]
public class RolesApiController : ControllerBase
{
    private readonly RoleStore<Role> _roleStore;
    private readonly ILogger<UsersApiController> _logger;

    public RolesApiController(WebStoreDB db, ILogger<UsersApiController> logger)
    {
        _roleStore = new(db);
        _logger = logger;
    }


    [HttpGet("all")]                                // GET /api/v1/roles/all HTTP/1.1
    public async Task<IEnumerable<Role>> GetAll()
        => await _roleStore.Roles.ToArrayAsync();


    [HttpPost]                                      // POST /api/v1/roles/ HTTP/1.1
    public async Task<bool> CreateAsync(Role role)
    {
        IdentityResult? report = await _roleStore.CreateAsync(role);

        if (!report.Succeeded)
        {
            _logger.LogWarning("Создание роли {0}: ошибка(и). [{1}]",
                role, string.Join("], [", report.Errors.Select(exc => exc.Description)));
        }
        return report.Succeeded;
    }


    [HttpPut]                                       // PUT /api/v1/roles/ HTTP/1.1
    public async Task<bool> UpdateAsync(Role role)
    {
        IdentityResult? report = await _roleStore.UpdateAsync(role);

        if (!report.Succeeded)
        {
            _logger.LogWarning("Изменение роли {0}: ошибка(и). [{1}]",
                role, string.Join("], [", report.Errors.Select(exc => exc.Description)));
        }
        return report.Succeeded;
    }


    [HttpDelete]                                    // DELETE /api/v1/roles/ HTTP/1.1
    [HttpPost("Delete")]                            // POST /api/v1/roles/delete HTTP/1.1
    public async Task<bool> DeleteAsync(Role role)
    {
        IdentityResult? report = await _roleStore.DeleteAsync(role);

        if (!report.Succeeded)
        {
            _logger.LogWarning("Удаление роли {0}: ошибка(и). [{1}]",
                role, string.Join("], [", report.Errors.Select(exc => exc.Description)));
        }
        return report.Succeeded;
    }


    [HttpPost("GetRoleId")]                         // POST /api/v1/roles/getroleid HTTP/1.1
    public async Task<string> GetRoleIdAsync([FromBody] Role role)
        => await _roleStore.GetRoleIdAsync(role);


    [HttpPost("GetRoleName")]                       // POST /api/v1/roles/getrolename HTTP/1.1
    public async Task<string> GetRoleNameAsync([FromBody] Role role)
        => await _roleStore.GetRoleNameAsync(role);


    [HttpPost("SetRoleName/{name}")]                // POST /api/v1/roles/setrolename/tsarj_bog_i_matj_priroda HTTP/1.1
    public async Task<string> SetRoleNameAsync(Role role, string name)
    {
        await _roleStore.SetRoleNameAsync(role, name);
        await _roleStore.UpdateAsync(role);
        return role.Name;
    }


    [HttpPost("GetNormalizedRoleName")]             // POST /api/v1/roles/getnormalizerrolename HTTP/1.1
    public async Task<string> GetNormalizedRoleNameAsync(Role role)
        => await _roleStore.GetNormalizedRoleNameAsync(role);


    [HttpPost("SetNormalizedRoleName/{name}")]      // POST /api/v1/roles/setnormalizedrolename/TSARJ_BOG_I_MATJ_PRIRODA HTTP/1.1
    public async Task<string> SetNormalizedRoleNameAsync(Role role, string name)
    {
        await _roleStore.SetNormalizedRoleNameAsync(role, name);
        await _roleStore.UpdateAsync(role);
        return role.NormalizedName;
    }


    [HttpGet("FindById/{id}")]                      // GET /api/v1/roles/findbyid/__ HTTP/1.1
    public async Task<Role> FindByIdAsync(string id)
        => await _roleStore.FindByIdAsync(id);


    [HttpGet("FindByName/{name}")]                  // GET /api/v1/roles/findbyname/tsarj_bog_i_matj_priroda HTTP/1.1
    public async Task<Role> FindByNameAsync(string name)
        => await _roleStore.FindByNameAsync(name);
}