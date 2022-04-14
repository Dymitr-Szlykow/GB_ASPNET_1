using System.Threading.Tasks;

namespace GB.ASPNET.WebStore.WebAPI.Clients;

public abstract class BaseClient
{
    protected HttpClient _HttpClient { get; init; }

    protected string _Address { get; init; }

    protected BaseClient(HttpClient httpClient, string address)
    {
        _HttpClient = httpClient;
        _Address = address;
    }
}