using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GB.ASPNET.WebStore.DAL.Context;
using GB.ASPNET.WebStore.Domain.Entities.Identity;
using GB.ASPNET.WebStore.Interfaces;

namespace GB.ASPNET.WebStore.WebAPI.Controllers.Identity;

[Route(WebApiRoutes.V1.IdentityUserClaimStoreRoute)]
[ApiController]
public class UserClaimApiController : ControllerBase
{
    private readonly ILogger<UserClaimApiController> _logger;

    public UserClaimApiController(WebStoreDB db, ILogger<UserClaimApiController> logger)
    {
        //_roleStore = new(db);
        _logger = logger;
    }
}