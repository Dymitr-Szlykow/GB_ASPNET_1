namespace GB.ASPNET.WebStore.Services.Interfaces;

public interface IVeiwAdminIndexData
{
    (string href, string FAStyles, string heading)[] Items { get; init; }
}