using System.Net.Http.Json;
using GB.ASPNET.WebStore.Interfaces;

namespace GB.ASPNET.WebStore.WebAPI.Clients.Values;

public class ValuesClient : BaseClient, IValuesAPI
{
    public ValuesClient(HttpClient httpClient) : base(httpClient, "api/values") { }

    public int Count()
    {
        HttpResponseMessage? response = _HttpClient.GetAsync($"{_Address}/count").Result;
        if (response.IsSuccessStatusCode)
            return response.Content.ReadFromJsonAsync<int>().Result!;
        else return -1;
    }

    public void Create(string newValue)
    {
        HttpResponseMessage? response = _HttpClient.PostAsJsonAsync(_Address, newValue).Result;
        response.EnsureSuccessStatusCode();
    }

    public bool Delete(int id)
    {
        HttpResponseMessage? response = _HttpClient.DeleteAsync($"{_Address}/{id}").Result;
        return response.IsSuccessStatusCode;
    }

    public IEnumerable<string> GetAll()
    {
        HttpResponseMessage? response = _HttpClient.GetAsync(_Address).Result;
        if (response.IsSuccessStatusCode)
            return response.Content.ReadFromJsonAsync<IEnumerable<string>>().Result!;
        else return Enumerable.Empty<string>();
    }

    public string? GetById(int id)
    {
        HttpResponseMessage? response = _HttpClient.GetAsync($"{_Address}/{id}").Result;
        if (response.IsSuccessStatusCode)
            return response.Content.ReadFromJsonAsync<string>().Result!;
        else return null;
    }

    public void Update(int id, string newValue)
    {
        HttpResponseMessage? response = _HttpClient.PutAsJsonAsync($"{_Address}/{id}", newValue).Result;
        response?.EnsureSuccessStatusCode();
    }
}