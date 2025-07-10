using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Data;

namespace MovieApp.API.Controllers
{
    /// <summary>
    /// Base application controller
    /// </summary>
    /// <param name="context">Context</param>
    /// <param name="mapper">Mapper</param>
    [ApiController]
    [Produces("application/json")]
    public class AppController(MovieContext context, IMapper mapper) : ControllerBase
    {
        protected readonly MovieContext _context = context
            ?? throw new ArgumentNullException(nameof(context));
        protected readonly IMapper _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
    }
}