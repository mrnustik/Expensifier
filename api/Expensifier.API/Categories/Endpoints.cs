using Expensifier.API.Categories.CreateCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Expensifier.API.Categories;

public static class Endpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        app.MapPost("/api/categories", 
            async ([FromBody]CreateCategoryCommand command, 
                IMediator mediator,
                CancellationToken ct) =>
            {
                var categoryId = await mediator.Send(command, ct);
                return TypedResults.Created($"api/categories/{categoryId}", categoryId);
            });

        app.MapGet("/api/categories",
                   async (IMediator mediator,
                          CancellationToken ct) =>
                   {
                       var categories = await mediator.Send(new GetCategories.GetCategories(), ct);
                       return TypedResults.Ok(categories);
                   });
    }
}