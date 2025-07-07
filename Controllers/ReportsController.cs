using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MovieApi.Data;
using MovieApi.Models.Dtos.Movie;
using MovieApi.Models.Dtos.Reports;
using MovieApi.Models.Entities;

namespace MovieApi.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController(MovieContext context, IMapper mapper)
        : AppController(context, mapper)
    {
        [HttpGet("top5pergenre")]
        public async Task<ActionResult<IEnumerable<TopMoviesByGenreDto>>> TopFiveByGenre()
        {
            var report = await _context
                .Movies.Where(m => m.Reviews.Any())
                .GroupBy(m => m.Genre.Name)
                .Select(g => new TopMoviesByGenreDto
                {
                    Genre = g.Key,
                    Movies = g.Select(m => new MovieWithRatingDto
                    {
                        Id = m.Id,
                        Title = m.Title,
                        Rating = m.Reviews.Average(r => r.Rating)
                    })
                    .OrderByDescending(m => m.Rating)
                    .Take(5)
                })
                .ToListAsync();

            return Ok(report);
        }

    }
}