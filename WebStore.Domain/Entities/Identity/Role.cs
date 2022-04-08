using Microsoft.AspNetCore.Identity;

namespace GB.ASPNET.WebStore.Domain.Entities.Identity;

public class Role : IdentityRole
{
    public const string administrators = "Administrators";
    public const string users = "Users";
}