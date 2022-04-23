using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GB.ASPNET.WebStore.DAL.Context;
using GB.ASPNET.WebStore.Domain.DataTransferObjects.Identity;
using GB.ASPNET.WebStore.Domain.Entities.Identity;
using GB.ASPNET.WebStore.Interfaces;

namespace GB.ASPNET.WebStore.WebAPI.Controllers;

[Route(WebApiRoutes.V1.IdentityUsersRoute)]
[ApiController]
public class UsersApiController : ControllerBase
{
    private readonly UserStore<User, Role, WebStoreDB> _userStore;
    private readonly ILogger<UsersApiController> _logger;

    public UsersApiController(WebStoreDB db, ILogger<UsersApiController> logger)
    {
        _userStore = new (db);
        _logger = logger;
    }


    [HttpGet("all")]                                    // GET /api/v1/users/all HTTP/1.1
    public async Task<IEnumerable<User>> GetAll()
        => await _userStore.Users.ToArrayAsync();


    #region Users

    [HttpPost("UserId")]                                // POST /api/v1/users/UserId HTTP/1.1
    public async Task<string> GetUserIdAsync([FromBody] User user)
        => await _userStore.GetUserIdAsync(user);


    [HttpPost("UserName")]                              // POST /api/v1/users/UserName HTTP/1.1
    public async Task<string> GetUserNameAsync([FromBody] User user)
        => await _userStore.GetUserNameAsync(user);


    [HttpPost("UserName/{name}")]                       // POST /api/v1/users/UserName/mrjohndoetheawesome HTTP/1.1
    public async Task<string> SetUserNameAsync([FromBody] User user, string name)
    {
        await _userStore.SetUserNameAsync(user, name);
        await _userStore.UpdateAsync(user);
        return user.UserName;
    }


    [HttpPost("NormalUserName")]                        // POST /api/v1/users/NormalUserName HTTP/1.1
    public async Task<string> GetNormalizedUserNameAsync([FromBody] User user)
        => await _userStore.GetNormalizedUserNameAsync(user);


    [HttpPost("NormalUserName/{name}")]                 // POST /api/v1/users/mrjohndoetheawesome HTTP/1.1
    public async Task<string> SetNormalizedUserNameAsync([FromBody] User user, string name)
    {
        await _userStore.SetNormalizedUserNameAsync(user, name);
        await _userStore.UpdateAsync(user);
        return user.NormalizedUserName;
    }


    [HttpPost("user")]                                  // POST /api/v1/users/user HTTP/1.1
    public async Task<bool> CreateAsync([FromBody] User user)
    {
        IdentityResult? report = await _userStore.CreateAsync(user);

        if (!report.Succeeded)
        {
            _logger.LogWarning("Создание пользователя {0}: ошибка(и). [{1}]",
                user, string.Join("], [", report.Errors.Select(exc => exc.Description)));
        }
        return report.Succeeded;
    }


    [HttpPut("user")]                                   // PUT /api/v1/users/user HTTP/1.1
    public async Task<bool> UpdateAsync([FromBody] User user)
    {
        IdentityResult? report = await _userStore.UpdateAsync(user);

        if (!report.Succeeded)
        {
            _logger.LogWarning("Редактирование пользователя {0}: ошибка(и). [{1}]",
                user, string.Join("], [", report.Errors.Select(exc => exc.Description)));
        }
        return report.Succeeded;
    }


    [HttpPost("user/delete")]                           // POST /api/v1/users/user/delete HTTP/1.1
    [HttpDelete("user/delete")]                         // DELETE /api/v1/users/user/delete HTTP/1.1
    [HttpDelete]                                        // DELETE /api/v1/users HTTP/1.1
    public async Task<bool> DeleteAsync([FromBody] User user)
    {
        IdentityResult? report = await _userStore.DeleteAsync(user);

        if (!report.Succeeded)
        {
            _logger.LogWarning("РУдаление пользователя {0}: ошибка(и). [{1}]",
                user, string.Join("], [", report.Errors.Select(exc => exc.Description)));
        }
        return report.Succeeded;
    }


    [HttpGet("user/find/{id}")]                         // GET /api/v1/users/user/find/9E5CB5E7-41DE-4449-829E-45F4C97AA54B HTTP/1.1
    public async Task<User> FindByIdAsync(string id)
        => await _userStore.FindByIdAsync(id);


    [HttpGet("User/normal/{name}")]                     // GET /api/v1/users/user/normal/mrjohndoetheawesome HTTP/1.1
    public async Task<User> FindByNameAsync(string name)
        => await _userStore.FindByNameAsync(name);


    [HttpPost("role/{role}")]                           // POST /api/v1/users/role/tsarj_bog_i_matj_priroda HTTP/1.1  // добавляет роль пользователю, который передаётся в теле запроса
    public async Task AddToRoleAsync([FromBody] User user, string role/*, [FromServices] WebStoreDB db*/)
    {
        await _userStore.AddToRoleAsync(user, role);
        await _userStore.Context.SaveChangesAsync();
        //await db.SaveChangesAsync();
    }


    [HttpDelete("role/{role}")]                         // DELETE /api/v1/users/role/tsarj_bog_i_matj_priroda HTTP/1.1
    [HttpPost("role/delete/{role}")]                    // POST /api/v1/users//roel/delete/tsarj_bog_i_matj_priroda HTTP/1.1
    public async Task RemoveFromRoleAsync([FromBody] User user, string role/*, [FromServices] WebStoreDB db - до .NET 5 */)
    {
        await _userStore.RemoveFromRoleAsync(user, role);
        await _userStore.Context.SaveChangesAsync(); // с .NET 5
        //await db.SaveChangesAsync();
    }


    [HttpPost("roles")]                                 // POST /api/v1/users/roles/ HTTP/1.1
    public async Task<IList<string>> GetRolesAsync([FromBody] User user)
        => await _userStore.GetRolesAsync(user);


    [HttpPost("inrole/{role}")]                         // POST /api/v1/users/inrole/tsarj_bog_i_matj_priroda HTTP/1.1
    public async Task<bool> IsInRoleAsync([FromBody] User user, string role)
        => await _userStore.IsInRoleAsync(user, role);


    [HttpGet("usersinrole/{role}")]                     // GET /api/v1/users/usersinrole/tsarj_bog_i_matj_priroda HTTP/1.1
    public async Task<IList<User>> GetUsersInRoleAsync(string role)
        => await _userStore.GetUsersInRoleAsync(role);


    [HttpPost("getpasswordhash")]                       // POST /api/v1/users/getpasswordhash HTTP/1.1
    public async Task<string> GetPasswordHashAsync([FromBody] User user)
        => await _userStore.GetPasswordHashAsync(user);


    [HttpPost("setpasswordhash")]                       // POST /api/v1/users/setpasswordhash HTTP/1.1
    public async Task<string> SetPasswordHashAsync([FromBody] UserPasswordHashDTO hash)
    {
        await _userStore.SetPasswordHashAsync(hash.User, hash.Hash);
        await _userStore.UpdateAsync(hash.User);
        return hash.User.PasswordHash;
    }


    [HttpPost("HasPassword")]                           // POST /api/v1/users/haspassword HTTP/1.1
    public async Task<bool> HasPasswordAsync([FromBody] User user)
        => await _userStore.HasPasswordAsync(user);

    #endregion


    #region Claims

    [HttpPost("GetClaims")]                             // POST /api/v1/users/getclaims HTTP/1.1
    public async Task<IList<Claim>> GetClaimsAsync([FromBody] User user)
        => await _userStore.GetClaimsAsync(user);


    [HttpPost("AddClaims")]                             // POST /api/v1/users/claims HTTP/1.1
    public async Task AddClaimsAsync([FromBody] ClaimDTO ClaimInfo/*, [FromServices] WebStoreDB db*/)
    {
        await _userStore.AddClaimsAsync(ClaimInfo.User, ClaimInfo.Claims);
        await _userStore.Context.SaveChangesAsync();
        //await db.SaveChangesAsync();
    }


    [HttpPost("ReplaceClaim")]                          // POST /api/v1/users/replaceclaims HTTP/1.1
    public async Task ReplaceClaimAsync([FromBody] ClaimReplacingDTO ClaimInfo/*, [FromServices] WebStoreDB db*/)
    {
        await _userStore.ReplaceClaimAsync(ClaimInfo.User, ClaimInfo.TargetClaim, ClaimInfo.NewClaim);
        await _userStore.Context.SaveChangesAsync();
        //await db.SaveChangesAsync();
    }


    [HttpPost("RemoveClaim")]                           // POST /api/v1/users/removeclaims HTTP/1.1
    public async Task RemoveClaimsAsync([FromBody] ClaimDTO ClaimInfo/*, [FromServices] WebStoreDB db*/)
    {
        await _userStore.RemoveClaimsAsync(ClaimInfo.User, ClaimInfo.Claims);
        await _userStore.Context.SaveChangesAsync();
        //await db.SaveChangesAsync();
    }


    [HttpPost("GetUsersForClaim")]                      // POST /api/v1/users/getusersforclaim HTTP/1.1
    public async Task<IList<User>> GetUsersForClaimAsync([FromBody] Claim claim)
        => await _userStore.GetUsersForClaimAsync(claim);

    #endregion


    #region TwoFactor

    [HttpPost("GetTwoFactorEnabled")]                   // POST /api/v1/users/gettwofactorenabled HTTP/1.1
    public async Task<bool> GetTwoFactorEnabledAsync([FromBody] User user)
        => await _userStore.GetTwoFactorEnabledAsync(user);


    [HttpPost("SetTwoFactor/{enable}")]                 // POST /api/v1/users/settwofactor/false HTTP/1.1
    public async Task<bool> SetTwoFactorEnabledAsync([FromBody] User user, bool enable)
    {
        await _userStore.SetTwoFactorEnabledAsync(user, enable);
        await _userStore.UpdateAsync(user);
        return user.TwoFactorEnabled;
    }

    #endregion


    #region Email/Phone

    [HttpPost("GetEmail")]                              // POST /api/v1/users/getemail HTTP/1.1
    public async Task<string> GetEmailAsync([FromBody] User user)
        => await _userStore.GetEmailAsync(user);


    [HttpPost("SetEmail/{email}")]                      // POST /api/v1/users/setemail/__ HTTP/1.1
    public async Task<string> SetEmailAsync([FromBody] User user, string email)
    {
        await _userStore.SetEmailAsync(user, email);
        await _userStore.UpdateAsync(user);
        return user.Email;
    }


    [HttpPost("GetNormalizedEmail")]                    // POST /api/v1/users/getnormalizedemail HTTP/1.1
    public async Task<string> GetNormalizedEmailAsync([FromBody] User user)
        => await _userStore.GetNormalizedEmailAsync(user);


    [HttpPost("SetNormalizedEmail/{email?}")]           // POST /api/v1/users/setnormalizedemail/__ HTTP/1.1
    public async Task<string> SetNormalizedEmailAsync([FromBody] User user, string? email)
    {
        await _userStore.SetNormalizedEmailAsync(user, email);
        await _userStore.UpdateAsync(user);
        return user.NormalizedEmail;
    }


    [HttpPost("GetEmailConfirmed")]                     // POST /api/v1/users/getemailconfirmed HTTP/1.1
    public async Task<bool> GetEmailConfirmedAsync([FromBody] User user)
        => await _userStore.GetEmailConfirmedAsync(user);


    [HttpPost("SetEmailConfirmed/{enable}")]            // POST /api/v1/users/setemailconfirmed/true HTTP/1.1
    public async Task<bool> SetEmailConfirmedAsync([FromBody] User user, bool enable)
    {
        await _userStore.SetEmailConfirmedAsync(user, enable);
        await _userStore.UpdateAsync(user);
        return user.EmailConfirmed;
    }


    [HttpGet("UserFindByEmail/{email}")]                // GET /api/v1/users/userfindbyemail/__ HTTP/1.1
    public async Task<User> FindByEmailAsync(string email)
        => await _userStore.FindByEmailAsync(email);


    [HttpPost("GetPhoneNumber")]                        // POST /api/v1/users/getphonenumber HTTP/1.1
    public async Task<string> GetPhoneNumberAsync([FromBody] User user)
        => await _userStore.GetPhoneNumberAsync(user);


    [HttpPost("SetPhoneNumber/{phone}")]                // POST /api/v1/users/setphonenumber/95489031744 HTTP/1.1
    public async Task<string> SetPhoneNumberAsync([FromBody] User user, string phone)
    {
        await _userStore.SetPhoneNumberAsync(user, phone);
        await _userStore.UpdateAsync(user);
        return user.PhoneNumber;
    }


    [HttpPost("GetPhoneNumberConfirmed")]               // POST /api/v1/users/getphonenumberconfirmed HTTP/1.1
    public async Task<bool> GetPhoneNumberConfirmedAsync([FromBody] User user)
        => await _userStore.GetPhoneNumberConfirmedAsync(user);


    [HttpPost("SetPhoneNumberConfirmed/{confirmed}")]   // POST /api/v1/users/setphonenumberconfirmed/true HTTP/1.1
    public async Task<bool> SetPhoneNumberConfirmedAsync([FromBody] User user, bool confirmed)
    {
        await _userStore.SetPhoneNumberConfirmedAsync(user, confirmed);
        await _userStore.UpdateAsync(user);
        return user.PhoneNumberConfirmed;
    }

    #endregion


    #region Login/Lockout

    [HttpPost("AddLogin")]                                      // POST /api/v1/users/addlogin HTTP/1.1
    public async Task AddLoginAsync([FromBody] UserLoginAddingDTO login/*, [FromServices] WebStoreDB db*/)
    {
        await _userStore.AddLoginAsync(login.User, login.UserLoginInfo);
        await _userStore.Context.SaveChangesAsync();
        //await db.SaveChangesAsync();
    }


    [HttpPost("RemoveLogin/{LoginProvider}/{ProviderKey}")]     // POST /api/v1/users/removelogin/__/__ HTTP/1.1
    public async Task RemoveLoginAsync([FromBody] User user, string LoginProvider, string ProviderKey/*, [FromServices] WebStoreDB db*/)
    {
        await _userStore.RemoveLoginAsync(user, LoginProvider, ProviderKey);
        await _userStore.Context.SaveChangesAsync();
        //await db.SaveChangesAsync();
    }


    [HttpPost("GetLogins")]                                     // POST /api/v1/users/getlogins HTTP/1.1
    public async Task<IList<UserLoginInfo>> GetLoginsAsync([FromBody] User user)
        => await _userStore.GetLoginsAsync(user);


    [HttpGet("User/FindByLogin/{LoginProvider}/{ProviderKey}")]     // POST /api/v1/users/user/findbylogin/__/__ HTTP/1.1
    public async Task<User> FindByLoginAsync(string LoginProvider, string ProviderKey)
        => await _userStore.FindByLoginAsync(LoginProvider, ProviderKey);


    [HttpPost("GetLockoutEndDate")]                                 // POST /api/v1/users/getlockoutenddate HTTP/1.1
    public async Task<DateTimeOffset?> GetLockoutEndDateAsync([FromBody] User user)
        => await _userStore.GetLockoutEndDateAsync(user);


    [HttpPost("SetLockoutEndDate")]                                 // POST /api/v1/users/setlockoutenddate HTTP/1.1
    public async Task<DateTimeOffset?> SetLockoutEndDateAsync([FromBody] UserLockoutSettingDTO LockoutInfo)
    {
        await _userStore.SetLockoutEndDateAsync(LockoutInfo.User, LockoutInfo.LockoutEnd);
        await _userStore.UpdateAsync(LockoutInfo.User);
        return LockoutInfo.User.LockoutEnd;
    }


    [HttpPost("IncrementAccessFailedCount")]                        // POST /api/v1/users/incrementaccessfailedcount HTTP/1.1
    public async Task<int> IncrementAccessFailedCountAsync([FromBody] User user)
    {
        var count = await _userStore.IncrementAccessFailedCountAsync(user);
        await _userStore.UpdateAsync(user);
        return count;
    }


    [HttpPost("ResetAccessFailedCount")]                            // POST /api/v1/users/resetaccessfailedcount HTTP/1.1
    public async Task<int> ResetAccessFailedCountAsync([FromBody] User user)
    {
        await _userStore.ResetAccessFailedCountAsync(user);
        await _userStore.UpdateAsync(user);
        return user.AccessFailedCount;
    }


    [HttpPost("GetAccessFailedCount")]                              // POST /api/v1/users/getaccessfailedcount HTTP/1.1
    public async Task<int> GetAccessFailedCountAsync([FromBody] User user)
        => await _userStore.GetAccessFailedCountAsync(user);


    [HttpPost("GetLockoutEnabled")]                                 // POST /api/v1/users/getlockoutenabled HTTP/1.1
    public async Task<bool> GetLockoutEnabledAsync([FromBody] User user)
        => await _userStore.GetLockoutEnabledAsync(user);


    [HttpPost("SetLockoutEnabled/{enable}")]                        // POST /api/v1/users/setlockoutenabled/false HTTP/1.1
    public async Task<bool> SetLockoutEnabledAsync([FromBody] User user, bool enable)
    {
        await _userStore.SetLockoutEnabledAsync(user, enable);
        await _userStore.UpdateAsync(user);
        return user.LockoutEnabled;
    }

    #endregion
}