using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using GB.ASPNET.WebStore.Domain.DataTransferObjects.Identity;
using GB.ASPNET.WebStore.Domain.Entities.Identity;
using GB.ASPNET.WebStore.Interfaces;

namespace GB.ASPNET.WebStore.WebAPI.Clients.Identity;

public class UserLoginStoreClient : IdentityBaseClient, IUserLoginStore
{
    private readonly ILogger<UserLoginStoreClient> _logger;

    public UserLoginStoreClient(HttpClient httpClient, ILogger<UserLoginStoreClient> logger) : base(httpClient, WebApiRoutes.V1.IdentityUserLoginStoreRoute)
    {
        _logger = logger;
    }


    #region IUserLoginStore<User>

    public async Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken token)
    {
        await PostAsync(
            $"{ControllerRoute}/AddLogin",
            new UserLoginAddingDTO { User = user, UserLoginInfo = login },
            token).ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task RemoveLoginAsync(User user, string LoginProvider, string ProviderKey, CancellationToken token)
    {
        await PostAsync($"{ControllerRoute}/RemoveLogin/{LoginProvider}/{ProviderKey}", user, token)
            .ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken token)
    {
        HttpResponseMessage? response = await
            PostAsync($"{ControllerRoute}/GetLogins", user, token)
            .ConfigureAwait(continueOnCapturedContext: false);
        List<UserLoginInfo>? logins = await response
           .EnsureSuccessStatusCode()
           .Content.ReadFromJsonAsync<List<UserLoginInfo>>(cancellationToken: token)
           .ConfigureAwait(continueOnCapturedContext: false);
        return logins!;
    }

    public async Task<User> FindByLoginAsync(string LoginProvider, string ProviderKey, CancellationToken token)
    {
        User? user = await
            GetAsync<User>($"{ControllerRoute}/User/FindByLogin/{LoginProvider}/{ProviderKey}", token)
            .ConfigureAwait(continueOnCapturedContext: false);
        return user!;
    }

    #endregion
}
