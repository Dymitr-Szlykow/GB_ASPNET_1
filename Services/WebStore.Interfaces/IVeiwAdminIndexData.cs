namespace GB.ASPNET.WebStore.Interfaces;

public interface IVeiwAdminIndexData
{
    (string href, string FAStyles, string heading)[] Items { get; init; }
}