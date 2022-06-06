using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using GB.ASPNET.WebStore.Domain.DataTransferObjects.Identity;
using GB.ASPNET.WebStore.Domain.Entities.Identity;
using GB.ASPNET.WebStore.Interfaces;

namespace GB.ASPNET.WebStore.WebAPI.Clients;

public class UsersClient : BaseClient, IUsersClient
{
    private readonly ILogger<UsersClient> _logger;

    public UsersClient(HttpClient httpClient, ILogger<UsersClient> logger) : base(httpClient, WebApiRoutes.V1.IdentityUsersRoute)
    {
        _logger = logger;
    }


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


    #region Implementation of IUserRoleStore<User>

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


    #region IUserClaimStore<User>

    public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken token)
    {
        HttpResponseMessage? response = await
            PostAsync($"{ControllerRoute}/GetClaims", user, token)
            .ConfigureAwait(continueOnCapturedContext: false);
        List<Claim>? claims = await response
           .EnsureSuccessStatusCode()
           .Content.ReadFromJsonAsync<List<Claim>>(cancellationToken: token)
           .ConfigureAwait(continueOnCapturedContext: false);
        return claims!;
    }

    public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken token)
    {
        await PostAsync(
            $"{ControllerRoute}/AddClaims",
            new ClaimDTO { User = user, Claims = claims },
            token).ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task ReplaceClaimAsync(User user, Claim OldClaim, Claim NewClaim, CancellationToken token)
    {
        await PostAsync(
            $"{ControllerRoute}/ReplaceClaim",
            new ClaimReplacingDTO { User = user, TargetClaim = OldClaim, NewClaim = NewClaim },
            token).ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken token)
    {
        await PostAsync(
            $"{ControllerRoute}/RemoveClaims",
            new ClaimDTO { User = user, Claims = claims },
            token).ConfigureAwait(continueOnCapturedContext: false);
    }

    public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken token)
    {
        HttpResponseMessage? response = await
            PostAsync($"{ControllerRoute}/GetUsersForClaim", claim, token)
            .ConfigureAwait(continueOnCapturedContext: false);
        List<User>? users = await response
           .EnsureSuccessStatusCode()
           .Content.ReadFromJsonAsync<List<User>>(cancellationToken: token)
           .ConfigureAwait(continueOnCapturedContext: false);
        return users!;
    }

    #endregion
}