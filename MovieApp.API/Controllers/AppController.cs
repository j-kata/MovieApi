using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Core.Contracts;

namespace MovieApp.API.Controllers;

/// <summary>
/// Base application controller
/// </summary>
/// <param name="uow">UnitOfWork</param>
/// <param name="mapper">Mapper</param>
[ApiController]
[Produces("application/json")]
public class AppController(IUnitOfWork uow, IMapper mapper) : ControllerBase
{
    protected readonly IUnitOfWork uow = uow
        ?? throw new ArgumentNullException(nameof(uow));
    protected readonly IMapper mapper = mapper
        ?? throw new ArgumentNullException(nameof(mapper));
}
