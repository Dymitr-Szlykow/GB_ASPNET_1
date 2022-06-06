using System.Net.Http.Json;
using GB.ASPNET.WebStore.Interfaces;

namespace GB.ASPNET.WebStore.WebAPI.Clients;

public class ValuesClient : BaseClient, IValuesAPI
{
    public ValuesClient(HttpClient httpClient) : base(httpClient, WebApiRoutes.V1.ValuesRoute) { }

    public int Count()
    {
        HttpResponseMessage? response = HttpClient.GetAsync($"{ControllerRoute}/count").Result;
        if (response.IsSuccessStatusCode)
            return response.Content.ReadFromJsonAsync<int>().Result!;
        else return -1;
    }

    public void Create(string newValue)
    {
        HttpResponseMessage? response = HttpClient.PostAsJsonAsync(ControllerRoute, newValue).Result;
        response.EnsureSuccessStatusCode();
    }

    public bool Delete(int id)
    {
        HttpResponseMessage? response = HttpClient.DeleteAsync($"{ControllerRoute}/{id}").Result;
        return response.IsSuccessStatusCode;
    }

    public IEnumerable<string> GetAll()
    {
        HttpResponseMessage? response = HttpClient.GetAsync(ControllerRoute).Result;
        if (response.IsSuccessStatusCode)
            return response.Content.ReadFromJsonAsync<IEnumerable<string>>().Result!;
        else return Enumerable.Empty<string>();
    }

    public string? GetById(int id)
    {
        HttpResponseMessage? response = HttpClient.GetAsync($"{ControllerRoute}/{id}").Result;
        if (response.IsSuccessStatusCode)
            return response.Content.ReadFromJsonAsync<string>().Result!;
        else return null;
    }

    public void Update(int id, string newValue)
    {
        HttpResponseMessage? response = HttpClient.PutAsJsonAsync($"{ControllerRoute}/{id}", newValue).Result;
        response?.EnsureSuccessStatusCode();
    }
}