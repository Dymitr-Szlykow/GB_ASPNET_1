using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace GB.ASPNET.WebStore.Infrastructure.Conventions;

public class SetAreaControllersRoute : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        string? controllerNamespace = controller.ControllerType.Namespace;
        if (string.IsNullOrEmpty(controllerNamespace)) return;

        const string suffix = "Areas.";
        int index = controllerNamespace.IndexOf(suffix, StringComparison.OrdinalIgnoreCase);
        if (index == -1) return;

        index += suffix.Length;
        string? area = controllerNamespace[index..controllerNamespace.IndexOf('.', index)];
        if (string.IsNullOrEmpty(area)) return;

        if (controller.Attributes.OfType<AreaAttribute>().Any(a => a.RouteKey == "area" && a.RouteValue == area)) return;
        else controller.RouteValues["area"] = area;
    }
}