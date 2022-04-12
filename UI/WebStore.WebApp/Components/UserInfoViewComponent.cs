using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFramework;
using GB.ASPNET.WebStore.Domain.Entities.Identity;

namespace GB.ASPNET.WebStore.Components;

public class UserInfoViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
        => User.Identity?.IsAuthenticated == true
            ? View("UserInfo")
            : View();
}