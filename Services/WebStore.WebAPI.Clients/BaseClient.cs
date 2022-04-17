using System.Net;
using System.Net.Http.Json;

namespace GB.ASPNET.WebStore.WebAPI.Clients;

public abstract class BaseClient
{
    protected HttpClient HttpClient { get; init; }
    protected string ControllerRoute { get; init; }

    protected BaseClient(HttpClient httpClient, string controllerRoute)
    {
        HttpClient = httpClient;
        ControllerRoute = controllerRoute;
    }


    protected TModel? Get<TModel>(string url) => GetAsync<TModel>(url).Result;
    protected async Task<TModel?> GetAsync<TModel>(string url)
    {
        HttpResponseMessage? response = await HttpClient.GetAsync(url).ConfigureAwait(continueOnCapturedContext: false);
        response.EnsureSuccessStatusCode();
        if (response.StatusCode is HttpStatusCode.NoContent or HttpStatusCode.NotFound) return default;

        TModel? result = await response
            .Content.ReadFromJsonAsync<TModel>()
            .ConfigureAwait(continueOnCapturedContext: false);
        return result;
    }


    protected HttpResponseMessage Post<TSend>(string url, TSend obj) => PostAsync<TSend>(url, obj).Result;
    protected async Task<HttpResponseMessage> PostAsync<TSend>(string url, TSend obj)
    {
        HttpResponseMessage? response = await HttpClient.PostAsJsonAsync(url, obj).ConfigureAwait(continueOnCapturedContext: false);
        return response.EnsureSuccessStatusCode();
    }


    protected HttpResponseMessage Put<TSend>(string url, TSend obj) => PutAsync<TSend>(url, obj).Result;
    protected async Task<HttpResponseMessage> PutAsync<TSend>(string url, TSend obj)
    {
        HttpResponseMessage? response = await HttpClient.PutAsJsonAsync(url, obj).ConfigureAwait(continueOnCapturedContext: false);
        return response.EnsureSuccessStatusCode();
    }


    protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;
    protected async Task<HttpResponseMessage> DeleteAsync(string url)
    {
        HttpResponseMessage? response = await HttpClient.DeleteAsync(url).ConfigureAwait(continueOnCapturedContext: false);
        return response;
    }
}