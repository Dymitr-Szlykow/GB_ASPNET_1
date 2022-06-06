using System.Net;
using System.Net.Http.Json;

namespace GB.ASPNET.WebStore.WebAPI.Clients;

public abstract class BaseClient : IDisposable
{
    protected HttpClient HttpClient { get; init; }
    protected string ControllerRoute { get; init; }

    protected BaseClient(HttpClient httpClient, string controllerRoute)
    {
        HttpClient = httpClient;
        ControllerRoute = controllerRoute;
    }


    protected TModel? Get<TModel>(string url) => GetAsync<TModel>(url).Result;
    protected async Task<TModel?> GetAsync<TModel>(string url, CancellationToken token = default)
    {
        HttpResponseMessage? response = await HttpClient.GetAsync(url, token).ConfigureAwait(continueOnCapturedContext: false);
        response.EnsureSuccessStatusCode();
        if (response.StatusCode is HttpStatusCode.NoContent or HttpStatusCode.NotFound) return default;

        TModel? result = await response
            .Content.ReadFromJsonAsync<TModel>(cancellationToken: token)
            .ConfigureAwait(continueOnCapturedContext: false);
        return result;
    }


    protected HttpResponseMessage Post<TSend>(string url, TSend obj) => PostAsync<TSend>(url, obj).Result;
    protected async Task<HttpResponseMessage> PostAsync<TSend>(string url, TSend obj, CancellationToken token = default)
    {
        HttpResponseMessage? response = await HttpClient.PostAsJsonAsync(url, obj, token).ConfigureAwait(continueOnCapturedContext: false);
        return response.EnsureSuccessStatusCode();
    }


    protected HttpResponseMessage Put<TSend>(string url, TSend obj) => PutAsync<TSend>(url, obj).Result;
    protected async Task<HttpResponseMessage> PutAsync<TSend>(string url, TSend obj, CancellationToken token = default)
    {
        HttpResponseMessage? response = await HttpClient.PutAsJsonAsync(url, obj, token).ConfigureAwait(continueOnCapturedContext: false);
        return response.EnsureSuccessStatusCode();
    }


    protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;
    protected async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken token = default)
    {
        HttpResponseMessage? response = await HttpClient.DeleteAsync(url, token).ConfigureAwait(continueOnCapturedContext: false);
        return response;
    }


    //~BaseClient() => Dispose(false);
    public void Dispose()
    {
        if (_disposed) return;
        else
        {
            Dispose(_disposed = true);
            //GC.SuppressFinalize(this); // при финализаторе
        }
    }

    private bool _disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        else
        {
            if (disposing)
            {
                // высвободить управляемые ресурсы - то, что создано в конструкторе и позже
            }

            // высвободить неуправляемые ресурсы
        }
    }
}