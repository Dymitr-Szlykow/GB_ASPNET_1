using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GB.ASPNET.WebStore.Domain.DataTransferObjects;
using GB.ASPNET.WebStore.Domain.Entities.Orders;
using GB.ASPNET.WebStore.Interfaces;
using GB.ASPNET.WebStore.Models;

namespace GB.ASPNET.WebStore.WebAPI.Controllers;

[Route(WebApiRoutes.V1.OrdersRoute)]
[ApiController]
public class OrdersApiController : ControllerBase
{
    private readonly IOrderService _dataAccessor;
    private readonly ILogger _logger;

    public OrdersApiController(IOrderService orderService, ILogger logger)
    {
        _dataAccessor = orderService;
        _logger = logger;
    }


    [HttpPost("{userName}")]        // POST /localhost:5001/api/orders/mrjohndoetheawesome HTTP/1.1
    [HttpPost("user/{userName}")]   // POST /localhost:5001/api/orders/user/mrjohndoetheawesome HTTP/1.1
    public async Task<IActionResult> Create(string userName, [FromBody] CreateOrderDTO dto)
    {
        Order? result = await _dataAccessor.CreateOrderAsync(userName, dto.Items.ToCartVM(), dto.Order);
        if (result is null)
        {
            _logger.LogWarning("Ошибка создания заказа пользователя {userName} по модели: {dto}.", userName, dto);
            return BadRequest(result);
        }
        else
        {
            _logger.LogInformation("Создан заказ пользователя {userName} по модели: {dto}.", userName, dto);
            return CreatedAtAction(nameof(GetOrderById), new { result.Id }, result.ToDtoModel());
        }
    }


    [HttpGet("user/{userName}")]    // GET /localhost:5001/api/orders/user/mrjohndoetheawesome HTTP/1.1
    public async Task<IActionResult> GetUserOrders(string userName)
    {
        IEnumerable<Order>? result = await _dataAccessor.GetUserOrdersAsync(userName);
        return result is not null ? Ok(result.ToDtoModels()) : NotFound(userName);
    }


    [HttpGet("{id:int}")]           // GET /localhost:5001/api/orders/42 HTTP/1.1
    public async Task<IActionResult> GetOrderById(int id)
    {
        Order? result = await _dataAccessor.GetOrderByIdAsync(id);
        return result is not null ? Ok(result.ToDtoModel()) : NotFound(new { id });
    }
}