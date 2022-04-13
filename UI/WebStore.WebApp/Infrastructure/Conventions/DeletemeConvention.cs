using Microsoft.AspNetCore.Mvc.ApplicationModels;
namespace GB.ASPNET.WebStore.Infrastructure.Conventions;

public class DeletemeConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        if (controller.ControllerName == "Home")
        {
            //controller.Actions.Add(new ActionModel(...));
        }
    }
}
