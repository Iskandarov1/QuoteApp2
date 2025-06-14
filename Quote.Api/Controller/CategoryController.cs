using MediatR;
using Microsoft.AspNetCore.Mvc;
using Quote.Api.Contracts;
using Quote.Application.Category.Commands.CreateCategory;
using Quote.Application.Category.Commands.DeleteCategory;
using Quote.Application.Category.Commands.UpdateCategory;
using Quote.Application.Category.Queries.GetByIdCategory;
using Quote.Application.Category.Queries.GetCategories;
using Quote.Contracts.Common;
using Quote.Contracts.Requests.CategoryRequest;
using Quote.Contracts.Responses.CategoriesResponse;
using Quote.Domain.Core.Errors;
using Quote.Domain.Core.Primitives.Maybe;
using Quote.Domain.Core.Primitives.Result;

namespace Quote.Api.Controller;


[ApiController]
[Route("api/[controller]")]
public class CategoryController(IMediator mediator) : ApiController(mediator)
{
    [HttpGet()]
    [ProducesResponseType(typeof(PagedList<CategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<IActionResult> GetAll([FromQuery] GetCategoriesQuery request) =>
        await Maybe<GetCategoriesQuery>
            .From(request)
            .Bind(query => Mediator.Send(query, HttpContext.RequestAborted))
            .Match(Ok, NotFound);
    
    [HttpGet(ApiRoutes.Categories.GetById)]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<IActionResult> GetById(Guid Id) =>
        await Maybe<GetCategoryByIdQuery>
            .From(new GetCategoryByIdQuery(Id))
            .Bind(query => Mediator.Send(query, HttpContext.RequestAborted))
            .Match(Ok, NotFound);
    
    [HttpPost(ApiRoutes.Categories.Create)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> Create(CreateCategoryRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(request => new CreateCategoryCommand(request.Name))
            .Bind(command => Mediator.Send(command, HttpContext.RequestAborted))
            .Match(Ok, BadRequest);
    
    [HttpPut()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> Update(UpdateCategoryRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(request => new UpdateCategoryCommand(
                request.Id, request.Name))
            .Bind(command => Mediator.Send(command, HttpContext.RequestAborted))
            .Match(Ok, BadRequest);
    
    [HttpDelete(ApiRoutes.Categories.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> Delete(Guid id) =>
        await Result.Success(new DeleteCategoryCommand(id))
            .Bind(command => Mediator.Send(command, HttpContext.RequestAborted))
            .Match(Ok, BadRequest);
    
    
}