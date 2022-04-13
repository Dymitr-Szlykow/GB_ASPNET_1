﻿using Microsoft.AspNetCore.Mvc;
using GB.ASPNET.WebStore.Interfaces;
using GB.ASPNET.WebStore.WebAPI.Clients.Values;

namespace GB.ASPNET.WebStore.WebApp.Controllers;

public class WebApiController : Controller
{
    private readonly IValuesAPI _api;

    public WebApiController(IValuesAPI valuesApi) => _api = valuesApi;


    public IActionResult Index()
    {
        var values = _api.GetAll();
        return View(values);
    }
}