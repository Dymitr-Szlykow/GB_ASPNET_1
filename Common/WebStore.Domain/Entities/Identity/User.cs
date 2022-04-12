using Microsoft.AspNetCore.Identity;

namespace GB.ASPNET.WebStore.Domain.Entities.Identity;

public class User : IdentityUser
{
    public const string administrator = "Admin";
    public const string defaultAdminPassword = "aDPas_123yy";
}