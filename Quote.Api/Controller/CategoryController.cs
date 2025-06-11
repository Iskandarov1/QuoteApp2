using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Quote.Api.Contracts;
using Quote.Application.Category.Commands.CreateCategory;
using Quote.Application.Category.Commands.DeleteCategory;
using Quote.Application.Category.Commands.UpdateCategory;
using Quote.Application.Category.Queries.GetAllCategories;
using Quote.Application.Category.Queries.GetByIdCategory;
using Quote.Application.Quote.Commands.CreateQuote;
using Quote.Contracts.Common;
using Quote.Contracts.Requests.CategoriesRequest;
using Quote.Contracts.Requests.QuotesRequest;
using Quote.Contracts.Responses.CategoriesResponse;
using Quote.Contracts.Responses.QuotesResponse;
using Quote.Domain.Core.Errors;
using Quote.Domain.Core.Primitives.Maybe;
using Quote.Domain.Core.Primitives.Result;

namespace Quote.Api.Controller;


[ApiController]
[Microsoft.AspNetCore.Components.Route("api/[controller]")]
public class CategoryController(IMediator mediator) : ApiController(mediator)
{
    [HttpGet()]
    [ProducesResponseType(typeof(PagedList<CategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllCategoriesQuery request) =>
        await Maybe<GetAllCategoriesQuery>
            .From(request)
            .Bind(query => Mediator.Send(query, HttpContext.RequestAborted))
            .Match(Ok, NotFound);
    
    [HttpGet("{categoryId:guid}")]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<IActionResult> GetById(Guid categoryId) =>
        await Maybe<GetCategoryByIdQuery>
            .From(new GetCategoryByIdQuery(categoryId))
            .Bind(query => Mediator.Send(query, HttpContext.RequestAborted))
            .Match(Ok, NotFound);
    
    [HttpPost("category")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> Create(CreateCategoryRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(request => new CreateCategoryCommand(request.Name))
            .Bind(command => Mediator.Send(command, HttpContext.RequestAborted))
            .Match(Ok, BadRequest);
    
    [HttpPut("{categoryId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> Update(Guid categoryId, UpdateCategoryRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(request => new UpdateCategoryCommand(
                categoryId,
                request.Name))
            .Bind(command => Mediator.Send(command, HttpContext.RequestAborted))
            .Match(Ok, BadRequest);
    
    [HttpDelete("{categoryId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> Delete(Guid categoryId) =>
        await Result.Success(new DeleteCategoryCommand(categoryId))
            .Bind(command => Mediator.Send(command, HttpContext.RequestAborted))
            .Match(Ok, BadRequest);
    
    
}