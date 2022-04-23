using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GB.ASPNET.WebStore.Domain;
using GB.ASPNET.WebStore.Domain.DataTransferObjects;
using GB.ASPNET.WebStore.Models;
using GB.ASPNET.WebStore.Interfaces;

namespace GB.ASPNET.WebStore.WebAPI.Controllers;

[Route(WebApiRoutes.V1.CatalogRoute)]
[ApiController]
public class ProductsApiController : ControllerBase
{
    private readonly IProductData _dataAccessor;
    private readonly ILogger<ProductsApiController> _logger;

    public ProductsApiController(IProductData dataService, ILogger<ProductsApiController> logger)
    {
        _dataAccessor = dataService;
        _logger = logger;
    }


    [HttpGet("brands")]             // GET /localhost:5001/api/catalog/brands HTTP/1.1
    public IActionResult GetBrands()
    {
        IEnumerable<BrandDTO>? list = _dataAccessor.GetBrands().ToDtoModels();
        return list!.Any() ? Ok(list) : NoContent();
    }

    [HttpGet("brands/{id:int}")]    // GET /localhost:5001/api/catalog/brands/42 HTTP/1.1
    public IActionResult GetBrandById(int id)
    {
        BrandDTO? brand = _dataAccessor.GetBrandById(id).ToDtoModel();
        return brand is not null ? Ok(brand) : NotFound(new { id });
    }


    [HttpGet("products")]           // GET /localhost:5001/api/catalog/products HTTP/1.1  должен вернуть все товары
    [HttpPost("products")]          // POST /localhost:5001/api/catalog/products HTTP/1.1
    public IActionResult GetProducts([FromBody] ProductFilter? filter = null)
    {
        IEnumerable<ProductDTO?>? list = _dataAccessor.GetProducts(filter).ToDtoModels();
        return list!.Any() ? Ok(list) : NoContent();
    }

    [HttpGet("products/{id:int}")]  // GET /localhost:5001/api/catalog/products/42 HTTP/1.1
    public IActionResult GetProductById(int id)
    {
        ProductDTO? product = _dataAccessor.GetProductById(id).ToDtoModel();
        return product is not null ? Ok(product) : NotFound(new { id });
    }


    [HttpGet("sections")]           // GET /localhost:5001/api/catalog/sections HTTP/1.1
    public IActionResult GetSections()
    {
        IEnumerable<SectionDTO>? list = _dataAccessor.GetSections().ToDtoModels();
        return list!.Any() ? Ok(list) : NoContent();
    }

    [HttpGet("sections/{id:int}")]  // GET /localhost:5001/api/catalog/sections/42 HTTP/1.1
    public IActionResult GetSectionById(int id)
    {
        SectionDTO? section = _dataAccessor.GetSectionById(id).ToDtoModel();
        return section is not null ? Ok(section) : NotFound(new { id });
    }
}