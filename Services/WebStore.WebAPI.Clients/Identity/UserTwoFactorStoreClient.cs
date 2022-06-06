using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using GB.ASPNET.WebStore.Domain.DataTransferObjects.Identity;
using GB.ASPNET.WebStore.Domain.Entities.Identity;
using GB.ASPNET.WebStore.Interfaces;

namespace GB.ASPNET.WebStore.WebAPI.Clients.Identity;

public class UserTwoFactorStoreClient : IdentityBaseClient, IUserTwoFactorStore
{
    private readonly ILogger<UserTwoFactorStoreClient> _logger;

    public UserTwoFactorStoreClient(HttpClient httpClient, ILogger<UserTwoFactorStoreClient> logger) : base(httpClient, WebApiRoutes.V1.IdentityTwoFactorStoreRoute)
    {
        _logger = logger;
    }


    #region IUserTwoFactorStore<User>

    public async Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken token)
    {
        HttpResponseMessage? response = await
            PostAsync($"{ControllerRoute}/SetTwoFactor/{enabled}", user, token)
            .ConfigureAwait(continueOnCapturedContext: false);
        user.TwoFactorEnabled = await response
           .EnsureSuccessStatusCode()
           .Content.ReadFromJsonAsync<bool>(cancellationToken: token)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken token)
    {
        HttpResponseMessage? response = await
            PostAsync($"{ControllerRoute}/GetTwoFactorEnabled", user, token)
            .ConfigureAwait(continueOnCapturedContext: false);
        return await response
           .EnsureSuccessStatusCode()
           .Content.ReadFromJsonAsync<bool>(cancellationToken: token)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    #endregion
}