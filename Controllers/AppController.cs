using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Data;

namespace MovieApi.Controllers
{
    [ApiController]
    public class AppController : ControllerBase
    {
        protected readonly MovieContext _context;
        protected readonly IMapper _mapper;

        public AppController(MovieContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
    }
}