using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using GB.ASPNET.WebStore.Domain.Entities.Identity;

namespace GB.ASPNET.WebStore.WebAPI.Clients;

public abstract class IdentityBaseClient : BaseClient
{
    public IdentityBaseClient(HttpClient httpClient, string controllerRoute) : base(httpClient, controllerRoute) { }


    #region IUserStore<User>

    public async Task<string> GetUserIdAsync(User user, CancellationToken token)
    {
        HttpResponseMessage? response = await PostAsync($"{ControllerRoute}/fromthis/id", user, token);
        return await response
           .EnsureSuccessStatusCode()
           .Content.ReadAsStringAsync(token)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task<string> GetUserNameAsync(User user, CancellationToken token)
    {
        HttpResponseMessage? response = await PostAsync($"{ControllerRoute}/fromthis/name", user, token);
        return await response
           .EnsureSuccessStatusCode()
           .Content.ReadAsStringAsync(token)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task SetUserNameAsync(User user, string name, CancellationToken token)
    {
        HttpResponseMessage? response = await PostAsync($"{ControllerRoute}/tothis/name/{name}", user, token);
        user.UserName = await response
           .EnsureSuccessStatusCode()
           .Content.ReadAsStringAsync(token)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken token)
    {
        HttpResponseMessage? response = await PostAsync($"{ControllerRoute}/fromthis/normalizedname/", user, token);
        return await response
           .EnsureSuccessStatusCode()
           .Content.ReadAsStringAsync(token)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task SetNormalizedUserNameAsync(User user, string name, CancellationToken token)
    {
        HttpResponseMessage? response = await PostAsync($"{ControllerRoute}/tothis/normalizedname/{name}", user, token);
        user.NormalizedUserName = await response
           .EnsureSuccessStatusCode()
           .Content.ReadAsStringAsync(token)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task<IdentityResult> CreateAsync(User user, CancellationToken token)
    {
        HttpResponseMessage? response = await PostAsync($"{ControllerRoute}/new", user, token);
        bool creation_success = await response
           .EnsureSuccessStatusCode()
           .Content.ReadFromJsonAsync<bool>(cancellationToken: token)
           .ConfigureAwait(continueOnCapturedContext: false);
        return creation_success
            ? IdentityResult.Success
            : IdentityResult.Failed();
    }

    public async Task<IdentityResult> UpdateAsync(User user, CancellationToken token)
    {
        HttpResponseMessage? response = await PutAsync($"{ControllerRoute}/update", user, token);
        var update_result = await response
           .EnsureSuccessStatusCode()
           .Content.ReadFromJsonAsync<bool>(cancellationToken: token)
           .ConfigureAwait(continueOnCapturedContext: false);
        return update_result
            ? IdentityResult.Success
            : IdentityResult.Failed();
    }

    public async Task<IdentityResult> DeleteAsync(User user, CancellationToken token)
    {
        HttpResponseMessage? response = await PostAsync($"{ControllerRoute}/this/delete", user, token);
        var delete_result = await response
           .EnsureSuccessStatusCode()
           .Content.ReadFromJsonAsync<bool>(cancellationToken: token)
           .ConfigureAwait(continueOnCapturedContext: false);
        return delete_result
            ? IdentityResult.Success
            : IdentityResult.Failed();
    }

    public async Task<User> FindByIdAsync(string id, CancellationToken token)
    {
        User? user = await
            GetAsync<User>($"{ControllerRoute}/User/Find/{id}", token)
            .ConfigureAwait(continueOnCapturedContext: false);
        return user!;
    }

    public async Task<User> FindByNameAsync(string name, CancellationToken token)
    {
        User? user = await
            GetAsync<User>($"{ControllerRoute}/User/Normal/{name}", token)
            .ConfigureAwait(continueOnCapturedContext: false);
        return user!;
    }

    #endregion
}