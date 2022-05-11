using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using GB.ASPNET.WebStore.Domain.Entities.Identity;
using GB.ASPNET.WebStore.Interfaces;

namespace GB.ASPNET.WebStore.WebAPI.Clients.Identity;

public class UserEmailStoreClient : IdentityBaseClient, IUserEmailStore
{
    private readonly ILogger<UserEmailStoreClient> _logger;

    public UserEmailStoreClient(HttpClient httpClient, ILogger<UserEmailStoreClient> logger) : base(httpClient, WebApiRoutes.V1.IdentityUserEmailStoreRoute)
    {
        _logger = logger;
    }


    #region IUserEmailStore<User>

    public async Task SetEmailAsync(User user, string email, CancellationToken token)
    {
        HttpResponseMessage? response = await
            PostAsync($"{ControllerRoute}/SetEmail/{email}", user, token)
            .ConfigureAwait(continueOnCapturedContext: false);
        user.Email = await response
           .EnsureSuccessStatusCode()
           .Content.ReadAsStringAsync(token)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task<string> GetEmailAsync(User user, CancellationToken token)
    {
        HttpResponseMessage? response = await
            PostAsync($"{ControllerRoute}/GetEmail", user, token)
            .ConfigureAwait(continueOnCapturedContext: false);
        return await response
           .EnsureSuccessStatusCode()
           .Content.ReadAsStringAsync(token)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task<bool> GetEmailConfirmedAsync(User user, CancellationToken token)
    {
        HttpResponseMessage? response = await
            PostAsync($"{ControllerRoute}/GetEmailConfirmed", user, token)
            .ConfigureAwait(continueOnCapturedContext: false);
        return await response
           .EnsureSuccessStatusCode()
           .Content.ReadFromJsonAsync<bool>(cancellationToken: token)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken token)
    {
        HttpResponseMessage? response = await
            PostAsync($"{ControllerRoute}/SetEmailConfirmed/{confirmed}", user, token)
            .ConfigureAwait(continueOnCapturedContext: false);
        user.EmailConfirmed = await response
           .EnsureSuccessStatusCode()
           .Content.ReadFromJsonAsync<bool>(cancellationToken: token)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task<User> FindByEmailAsync(string email, CancellationToken token)
    {
        User? user = await
            GetAsync<User>($"{ControllerRoute}/User/FindByEmail/{email}", token)
            .ConfigureAwait(continueOnCapturedContext: false);
        return user!;
    }

    public async Task<string> GetNormalizedEmailAsync(User user, CancellationToken token)
    {
        HttpResponseMessage? response = await PostAsync($"{ControllerRoute}/User/GetNormalizedEmail", user, token);
        return await response
           .EnsureSuccessStatusCode()
           .Content.ReadAsStringAsync(token)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task SetNormalizedEmailAsync(User user, string email, CancellationToken token)
    {
        HttpResponseMessage? response = await
            PostAsync($"{ControllerRoute}/SetNormalizedEmail/{email}", user, token)
            .ConfigureAwait(continueOnCapturedContext: false);
        user.NormalizedEmail = await response
           .EnsureSuccessStatusCode()
           .Content.ReadAsStringAsync(token)
           .ConfigureAwait(continueOnCapturedContext: false);
    }

    #endregion
}