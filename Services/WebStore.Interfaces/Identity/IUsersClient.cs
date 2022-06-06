using Microsoft.AspNetCore.Identity;
using GB.ASPNET.WebStore.Domain.Entities.Identity;

namespace GB.ASPNET.WebStore.Interfaces;

public interface IUsersClient :
    IUserRoleStore<User>,
    IUserPasswordStore<User>,
    IUserEmailStore<User>,
    IUserPhoneNumberStore<User>,
    IUserTwoFactorStore<User>,
    IUserLoginStore<User>,
    IUserClaimStore<User>
{ }


public interface IUserClaimStore : IUserClaimStore<User> { }
public interface IUserEmailStore : IUserEmailStore<User> { }
public interface IUserLoginStore : IUserLoginStore<User> { }
public interface IUserPasswordStore : IUserPasswordStore<User> { }
public interface IUserPhoneNumberStore : IUserPhoneNumberStore<User> { }
public interface IUserRoleStore : IUserRoleStore<User> { }
public interface IUserTwoFactorStore : IUserTwoFactorStore<User> { }