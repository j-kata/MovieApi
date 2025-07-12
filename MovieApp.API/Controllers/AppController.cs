using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Contracts;
using MovieApp.Core.Contracts;
using MovieApp.Services;

namespace MovieApp.API.Controllers;

/// <summary>
/// Base application controller
/// </summary>
/// <param name="serviceManager">ServiceManager</param>
[ApiController]
[Produces("application/json")]
public class AppController(IServiceManager serviceManager) : ControllerBase
{
    protected IServiceManager ServiceManager { get; } = serviceManager
        ?? throw new ArgumentNullException(nameof(serviceManager));
}
