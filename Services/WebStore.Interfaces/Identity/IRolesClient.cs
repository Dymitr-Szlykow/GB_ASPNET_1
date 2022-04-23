using Microsoft.AspNetCore.Identity;
using GB.ASPNET.WebStore.Domain.Entities.Identity;

namespace GB.ASPNET.WebStore.Interfaces;

public interface IRolesClient : IRoleStore<Role> { }