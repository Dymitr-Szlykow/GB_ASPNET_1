using Microsoft.AspNetCore.Identity;
using GB.ASPNET.WebStore.Domain.Entities.Identity;

namespace GB.ASPNET.WebStore.Domain.DataTransferObjects.Identity;

public abstract class UserDTO
{
    public User User { get; set; } = null!;
}

public class UserLoginAddingDTO : UserDTO
{
    public UserLoginInfo UserLoginInfo { get; set; } = null!;
}

public class UserPasswordHashDTO : UserDTO
{
    public string Hash { get; set; } = null!;
}

public class UserLockoutSettingDTO : UserDTO
{
    public DateTimeOffset? LockoutEnd { get; set; } = null!;
}