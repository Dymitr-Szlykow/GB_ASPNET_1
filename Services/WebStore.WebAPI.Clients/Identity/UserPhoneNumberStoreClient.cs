using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using GB.ASPNET.WebStore.Domain.Entities.Identity;
using GB.ASPNET.WebStore.Interfaces;

namespace GB.ASPNET.WebStore.WebAPI.Clients.Identity;

public class UserPhoneNumberStoreClient : IdentityBaseClient, IUserPhoneNumberStore
{
    private readonly ILogger<UserPhoneNumberStoreClient> _logger;

    public UserPhoneNumberStoreClient(HttpClient httpClient, ILogger<UserPhoneNumberStoreClient> logger) : base(httpClient, WebApiRoutes.V1.IdentityPhoneNumberStoreRoute)
    {
        _logger = logger;
    }


    #region IUserPhoneNumberStore<User>

    public async Task SetPhoneNumberAsync(User user, string phone, CancellationToken token)
    {
        HttpResponseMessage? response = await
            PostAsync($"{ControllerRoute}/SetPhoneNumber/{phone}", user, token)
            .ConfigureAwait(continueOnCapturedContext: false);
        user.PhoneNumber = await response
           .EnsureSuccessStatusCode()
           .Content.ReadAsStringAsync(token)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task<string> GetPhoneNumberAsync(User user, CancellationToken token)
    {
        HttpResponseMessage? response = await
            PostAsync($"{ControllerRoute}/GetPhoneNumber", user, token)
            .ConfigureAwait(continueOnCapturedContext: false);
        return await response
           .EnsureSuccessStatusCode()
           .Content.ReadAsStringAsync(token)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken token)
    {
        HttpResponseMessage? response = await
            PostAsync($"{ControllerRoute}/GetPhoneNumberConfirmed", user, token)
            .ConfigureAwait(continueOnCapturedContext: false);
        return await response
           .EnsureSuccessStatusCode()
           .Content.ReadFromJsonAsync<bool>(cancellationToken: token)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken token)
    {
        HttpResponseMessage? response = await
            PostAsync($"{ControllerRoute}/SetPhoneNumberConfirmed/{confirmed}", user, token)
            .ConfigureAwait(continueOnCapturedContext: false);
        user.PhoneNumberConfirmed = await response
           .EnsureSuccessStatusCode()
           .Content.ReadFromJsonAsync<bool>(cancellationToken: token)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    #endregion
}