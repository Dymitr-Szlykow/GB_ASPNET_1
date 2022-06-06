using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using GB.ASPNET.WebStore.Domain.DataTransferObjects.Identity;
using GB.ASPNET.WebStore.Domain.Entities.Identity;
using GB.ASPNET.WebStore.Interfaces;

namespace GB.ASPNET.WebStore.WebAPI.Clients.Identity;

public class UserClaimStoreClient : IdentityBaseClient, IUserClaimStore
{
    private readonly ILogger<UserClaimStoreClient> _logger;

    public UserClaimStoreClient(HttpClient httpClient, ILogger<UserClaimStoreClient> logger) : base(httpClient, WebApiRoutes.V1.IdentityUserClaimStoreRoute)
    {
        _logger = logger;
    }


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