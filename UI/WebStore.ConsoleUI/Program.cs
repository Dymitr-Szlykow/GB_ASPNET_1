using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using GB.ASPNET.WebStore.Domain.Entities;
using GB.ASPNET.WebStore.WebAPI.Clients;

IConfigurationRoot? config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var http = new HttpClient() { BaseAddress = new Uri(config["WebAPI"]) };
var client = new ProductsClient(http);

Console.WriteLine("Ожидание запуска WebAPI... Нажмите Enter для продолжения.");
Console.ReadLine();

foreach (Product? item in client.GetProducts())
{
    Console.WriteLine($"[{item.Id}]: {item.Name};");
}

Console.ReadLine();