using GB.ASPNET.WebStore.ViewModels;

namespace GB.ASPNET.WebStore.Interfaces;

public interface ICart
{
    void Add(int id, int? num);
    void Clear();
    void RemoveOne(int id);
    void RemoveTitle(int id);

    CartVM GetViewmodel();
}