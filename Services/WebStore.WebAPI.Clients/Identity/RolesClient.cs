using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using GB.ASPNET.WebStore.Domain.Entities.Identity;
using GB.ASPNET.WebStore.Interfaces;

namespace GB.ASPNET.WebStore.WebAPI.Clients;

public class RolesClient : BaseClient, IRolesClient
{
    private readonly ILogger<RolesClient> _logger;

    public RolesClient(HttpClient httpClient, ILogger<RolesClient> logger) : base(httpClient, WebApiRoutes.V1.IdentityRolesRoute)
    {
        _logger = logger;
    }


    #region IRoleStore<Role>

    public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancel)
    {
        HttpResponseMessage? response = await PostAsync(ControllerRoute, role, cancel).ConfigureAwait(false);
        bool result = await response
           .EnsureSuccessStatusCode()
           .Content.ReadFromJsonAsync<bool>(cancellationToken: cancel)
           .ConfigureAwait(continueOnCapturedContext: false);
        return result
            ? IdentityResult.Success
            : IdentityResult.Failed();
    }

    public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancel)
    {
        HttpResponseMessage? response = await PutAsync(ControllerRoute, role, cancel).ConfigureAwait(false);
        bool result = await response
           .EnsureSuccessStatusCode()
           .Content.ReadFromJsonAsync<bool>(cancellationToken: cancel)
           .ConfigureAwait(false);
        return result
            ? IdentityResult.Success
            : IdentityResult.Failed();
    }

    public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancel)
    {
        HttpResponseMessage? response = await PostAsync($"{ControllerRoute}/Delete", role, cancel).ConfigureAwait(false);
        bool result = await response
           .EnsureSuccessStatusCode()
           .Content.ReadFromJsonAsync<bool>(cancellationToken: cancel)
           .ConfigureAwait(false);
        return result
            ? IdentityResult.Success
            : IdentityResult.Failed();
    }

    public async Task<string> GetRoleIdAsync(Role role, CancellationToken cancel)
    {
        HttpResponseMessage? response = await PostAsync($"{ControllerRoute}/GetRoleId", role, cancel).ConfigureAwait(false);
        return await response
           .EnsureSuccessStatusCode()
           .Content.ReadAsStringAsync(cancel)
           .ConfigureAwait(false);
    }

    public async Task<string> GetRoleNameAsync(Role role, CancellationToken cancel)
    {
        HttpResponseMessage? response = await PostAsync($"{ControllerRoute}/GetRoleName", role, cancel).ConfigureAwait(false);
        return await response
           .EnsureSuccessStatusCode()
           .Content.ReadAsStringAsync(cancel)
           .ConfigureAwait(false);
    }

    public async Task SetRoleNameAsync(Role role, string name, CancellationToken cancel)
    {
        HttpResponseMessage? response = await PostAsync($"{ControllerRoute}/SetRoleName/{name}", role, cancel).ConfigureAwait(false);
        role.Name = await response
           .EnsureSuccessStatusCode()
           .Content.ReadAsStringAsync(cancel)
           .ConfigureAwait(false);
    }

    public async Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancel)
    {
        HttpResponseMessage? response = await PostAsync($"{ControllerRoute}/GetNormalizedRoleName", role, cancel).ConfigureAwait(false);
        return await response
           .EnsureSuccessStatusCode()
           .Content.ReadAsStringAsync(cancel)
           .ConfigureAwait(false);
    }

    public async Task SetNormalizedRoleNameAsync(Role role, string name, CancellationToken cancel)
    {
        HttpResponseMessage? response = await PostAsync($"{ControllerRoute}/SetNormalizedRoleName/{name}", role, cancel).ConfigureAwait(false);
        role.NormalizedName = await response
           .EnsureSuccessStatusCode()
           .Content.ReadAsStringAsync(cancel)
           .ConfigureAwait(false);
    }

    public async Task<Role> FindByIdAsync(string id, CancellationToken cancel)
    {
        Role? role = await GetAsync<Role>($"{ControllerRoute}/FindById/{id}", cancel).ConfigureAwait(false);
        return role!;
    }

    public async Task<Role> FindByNameAsync(string name, CancellationToken cancel)
    {
        Role? role = await GetAsync<Role>($"{ControllerRoute}/FindByName/{name}", cancel).ConfigureAwait(false);
        return role!;
    }

    #endregion
}