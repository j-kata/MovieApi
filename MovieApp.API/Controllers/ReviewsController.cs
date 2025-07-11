using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Core.Entities;
using MovieApp.Core.Contracts;

namespace MovieApp.API.Controllers;

/// <summary>
/// Reviews controller
/// </summary>
/// <param name="uow">UnitOfWork</param>
/// <param name="mapper">Mapper</param>
[Route("api/reviews/{id}")]
public class ReviewsController(IUnitOfWork uow, IMapper mapper)
    : AppController(uow, mapper)
{
    /// <summary>
    /// Delete review by id
    /// </summary>
    /// <param name="id">Id of review</param>
    /// <returns>No content if successful, or 404 if not found</returns>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReview(int id)
    {
        if (!await uow.Reviews.AnyByIdAsync(id))
            return NotFound();

        uow.Reviews.RemoveById(id);

        await uow.CompleteAsync();

        return NoContent();
    }
}
