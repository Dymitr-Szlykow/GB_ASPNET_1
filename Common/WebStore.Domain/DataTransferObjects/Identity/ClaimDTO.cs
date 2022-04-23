using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using GB.ASPNET.WebStore.Domain.Entities.Identity;

namespace GB.ASPNET.WebStore.Domain.DataTransferObjects.Identity;

public class ClaimDTO : UserDTO
{
    public IEnumerable<Claim> Claims { get; set; } = null!;
}

public class ClaimReplacingDTO : UserDTO
{
    public Claim TargetClaim { get; set; } = null!;
    public Claim NewClaim { get; set; } = null!;
}