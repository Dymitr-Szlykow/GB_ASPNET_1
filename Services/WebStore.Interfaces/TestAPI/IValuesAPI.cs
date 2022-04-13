namespace GB.ASPNET.WebStore.Interfaces;

public interface IValuesAPI
{
    int Count();

    void Create(string newValue);

    bool Delete(int id);

    string? GetById(int id);

    IEnumerable<string> GetAll();

    void Update(int id, string newValue);
}