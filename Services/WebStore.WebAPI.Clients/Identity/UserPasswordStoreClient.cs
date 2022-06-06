using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using GB.ASPNET.WebStore.Domain.DataTransferObjects.Identity;
using GB.ASPNET.WebStore.Domain.Entities.Identity;
using GB.ASPNET.WebStore.Interfaces;

namespace GB.ASPNET.WebStore.WebAPI.Clients.Identity;

public class UserPasswordStoreClient : IdentityBaseClient, IUserPasswordStore
{
    private readonly ILogger<UserPasswordStoreClient> _logger;

    public UserPasswordStoreClient(HttpClient httpClient, ILogger<UserPasswordStoreClient> logger) : base(httpClient, WebApiRoutes.V1.IdentityUserPasswordStoreRoute)
    {
        _logger = logger;
    }


    #region IUserPasswordStore<User>

    public async Task SetPasswordHashAsync(User user, string hash, CancellationToken token)
    {
        HttpResponseMessage? response = await PostAsync(
            $"{ControllerRoute}/SetPasswordHash",
            new UserPasswordHashDTO { User = user, Hash = hash },
            token).ConfigureAwait(continueOnCapturedContext: false);
        user.PasswordHash = await response
           .EnsureSuccessStatusCode()
           .Content.ReadAsStringAsync(token);
        //user.PasswordHash = await response.Content.ReadAsStringAsync(token).ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task<string> GetPasswordHashAsync(User user, CancellationToken token)
    {
        HttpResponseMessage? response = await
            PostAsync($"{ControllerRoute}/GetPasswordHash", user, token)
            .ConfigureAwait(continueOnCapturedContext: false);
        return await response
           .EnsureSuccessStatusCode()
           .Content.ReadAsStringAsync(token)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task<bool> HasPasswordAsync(User user, CancellationToken token)
    {
        HttpResponseMessage? response = await
            PostAsync($"{ControllerRoute}/HasPassword", user, token)
            .ConfigureAwait(continueOnCapturedContext: false);
        return await response
           .EnsureSuccessStatusCode()
           .Content.ReadFromJsonAsync<bool>(cancellationToken: token)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    #endregion
}