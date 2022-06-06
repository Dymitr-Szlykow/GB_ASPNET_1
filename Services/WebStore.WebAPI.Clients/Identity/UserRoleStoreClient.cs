using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using GB.ASPNET.WebStore.Interfaces;
using GB.ASPNET.WebStore.Domain.Entities.Identity;

namespace GB.ASPNET.WebStore.WebAPI.Clients.Identity;

public class UserRoleStoreClient : IdentityBaseClient, IUserRoleStore
{
    private readonly ILogger<UserRoleStoreClient> _logger;

    public UserRoleStoreClient(HttpClient httpClient, ILogger<UserRoleStoreClient> logger) : base(httpClient, WebApiRoutes.V1.IdentityUserRoleStoreRoute)
    {
        _logger = logger;
    }


    #region IUserRoleStore<User>

    public async Task AddToRoleAsync(User user, string role, CancellationToken token)
    {
        await PostAsync($"{ControllerRoute}/Role/{role}", user, token)
            .ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task RemoveFromRoleAsync(User user, string role, CancellationToken token)
    {
        await PostAsync($"{ControllerRoute}/Role/Delete/{role}", user, token)
            .ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task<IList<string>> GetRolesAsync(User user, CancellationToken token)
    {
        HttpResponseMessage? response = await PostAsync($"{ControllerRoute}/roles", user, token)
            .ConfigureAwait(continueOnCapturedContext: false);
        IList<string>? roles = await response
           .EnsureSuccessStatusCode()
           .Content.ReadFromJsonAsync<IList<string>>(cancellationToken: token)
           .ConfigureAwait(continueOnCapturedContext: false);
        return roles!;
    }

    public async Task<bool> IsInRoleAsync(User user, string role, CancellationToken token)
    {
        HttpResponseMessage? response = await PostAsync($"{ControllerRoute}/InRole/{role}", user, token);
        return await response
           .EnsureSuccessStatusCode()
           .Content.ReadFromJsonAsync<bool>(cancellationToken: token)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task<IList<User>> GetUsersInRoleAsync(string role, CancellationToken token)
    {
        List<User>? users = await
            GetAsync<List<User>>($"{ControllerRoute}/UsersInRole/{role}", token)
            .ConfigureAwait(continueOnCapturedContext: false);
        return users!;
    }

    #endregion
}