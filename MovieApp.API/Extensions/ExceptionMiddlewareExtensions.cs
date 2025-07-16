using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MovieApp.Core.Exceptions;

namespace MovieApp.API.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(builder =>
            builder.Run(async context =>
            {
                var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                if (error is null) return;

                var problemDetailsFactory = app.ApplicationServices.GetRequiredService<ProblemDetailsFactory>();

                var statusCode = GetStatusCode(error);
                var title = GetTitle(error);
                var detail = error.Message;

                var problemDetails = problemDetailsFactory.CreateProblemDetails(
                    context,
                    statusCode: statusCode,
                    title: title,
                    detail: detail,
                    instance: context.Request.Path);

                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        ));
    }

    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            INotFoundException => StatusCodes.Status404NotFound,
            ConflictException => StatusCodes.Status409Conflict,
            BadRequestException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

    private static string GetTitle(Exception exception) =>
        exception is ApiException customError ? customError.Title : "Internal Server Error";
}