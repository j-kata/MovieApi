using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Core.Shared;

namespace MovieApp.Presentation.Extensions;

public static class ControllerExtensions
{
    public static void IncludePaginationMeta(this ControllerBase controller, PaginationMeta meta)
    {
        controller.Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(meta));
        controller.Response.Headers.Append("Access-Control-Expose-Headers", "X-Pagination");
    }
}