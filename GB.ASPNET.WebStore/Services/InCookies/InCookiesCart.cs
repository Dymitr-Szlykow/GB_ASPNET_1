using System.Security.Claims;
using Newtonesoft.Json;
using GB.ASPNET.WebStore.Domain;
using GB.ASPNET.WebStore.Domain.Entities;
using GB.ASPNET.WebStore.Models;
using GB.ASPNET.WebStore.ViewModels;
using GB.ASPNET.WebStore.Services.Interfaces;

namespace GB.ASPNET.WebStore.Services;

public class InCookiesCart : ICart
{
    private readonly IHttpContextAccessor _HttpAccessor;
    private readonly IProductData _ProductData;
    private readonly ILogger<InCookiesCart> _logger;
    private readonly string _CartName;

    private Cart Cart
    {
        get
        {
            IResponseCookies? cookiesIn = _HttpAccessor.HttpContext!.Response.Cookies;
            string? сookiesOut = _HttpAccessor.HttpContext.Request.Cookies[_CartName];

            if (сookiesOut is null)
            {
                var cart = new Cart();
                cookiesIn.Append(_CartName, JsonConvert.SerializeObject(cart));
                return cart;
            }
            else {
                ReplaceCart(cookiesIn, сookiesOut);
                return JsonConvert.DeserializeObject<Cart>(сookiesOut);
            }
        }
        set => ReplaceCart(
            _HttpAccessor.HttpContext!.Response.Cookies,
            JsonConvert.SerializeObject(value));
    }


    public InCookiesCart(IHttpContextAccessor httpContextAccessor, IProductData productData, ILogger<InCookiesCart> logger)
    {
        _HttpAccessor = httpContextAccessor;
        _ProductData = productData;
        _logger = logger;

        ClaimsPrincipal? user = httpContextAccessor.HttpContext!.User;
        string? userName = user.Identity?.IsAuthenticated == true ? $"-{user.Identity.Name}" : null;
        _CartName = $"WebStore.GB.Cart{userName}";
    }


    private void ReplaceCart(IResponseCookies cookies, string jsonCart)
    {
        cookies.Delete(_CartName);
        cookies.Append(_CartName, jsonCart);
    }

    public void Add(int id, int? num)
    {
        Cart? cart = Cart;

        CartItem item = cart.FirstOrDefault(one => one.ProductId = id);
        if (item is null) cart.Items.Add(item = new CartItem() { ProductId = id, Quantity = 0 });
        if (num.HasValue) item.Quantity += num.Value;
        else item.Quantity++;

        Cart = cart;
    }

    public void Clear()
    {
        Cart? cart = Cart;
        cart.Items.Clear();
        Cart = cart;
    }

    public CartVM GetViewmodel()
    {
        Cart? cart = Cart;
        IEnumerable<Product>? products = _ProductData.GetProducts(
            new ProductFilter()
            {
                Ids = cart.Items.Select(item => item.ProductId).ToArray(),
            });

        Dictionary<int,ProductViewModel?> viewmodels = products.ToViewmodels().ToDictionary(el => el!.Id);
        return new CartVM
        {
            Items = cart.Items
                .Where(item => viewmodels.ContainsKey(item.ProductId))
                .Select(item => (viewmodels[item.ProductId]!, item.Quantity))
        };
    }

    public void RemoveOne(int id)
    {
        Cart? cart = Cart;

        CartItem item = cart.FirstOrDefault(one => one.ProductId = id);
        if (item is not null)
        {
            if (--item.Quantity <= 0) cart.Items.Remove(item);
            Cart = cart;
        }
        else _logger.LogWarning(
            "Попытка убрать из корзины единицу не обнаруженнго в ней товара. Имя корзины: {0}, id товара: {1}, см.корзину в параметрах",
            _CartName, id, cart);
    }

    public void RemoveTitle(int id)
    {
        Cart? cart = Cart;

        CartItem item = cart.FirstOrDefault(one => one.ProductId = id);
        if (item is not null)
        {
            cart.Items.Remove(item);
            Cart = cart;
        }
        else _logger.LogWarning(
            "Попытка удалить из корзины не обнаруженное в ней наименование товара. Имя корзины: {0}, id товара: {1}, см.корзину в параметрах",
            _CartName, id, cart);
    }
}