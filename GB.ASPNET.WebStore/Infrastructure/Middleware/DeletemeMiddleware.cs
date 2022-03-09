namespace GB.ASPNET.WebStore.Infrastructure.Middleware;

public class DeletemeMiddleware
{
    private readonly RequestDelegate _next;

    public DeletemeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        //var routeValueDictionary = context.Request.RouteValues;
        //var iQueryCollection = context.Request.Query;
        //var iHeaderDictionary = context.Request.Headers;
        //var iRequestCookieCollection = context.Request.Cookies;

        await _next(context);

        //_ = context.Response.Headers;
        //_ = context.Response.StatusCode;
    }
}
