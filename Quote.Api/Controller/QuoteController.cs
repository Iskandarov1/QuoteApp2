using MediatR;
using Microsoft.AspNetCore.Mvc;
using Quote.Api.Contracts;
using Quote.Application.Category.Commands.DeleteCategory;
using Quote.Application.Quote.Commands.CreateQuote;
using Quote.Application.Quote.Commands.DeleteQuote;
using Quote.Application.Quote.Commands.UpdateQuote;
using Quote.Application.Quote.Queries.GetAllQuotes;
using Quote.Application.Quote.Queries.GetByIdQuote;
using Quote.Application.Quote.Queries.GetRandomQuote;
using Quote.Contracts.Common;
using Quote.Contracts.Requests.QuotesRequest;
using Quote.Contracts.Responses.QuotesResponse;
using Quote.Domain.Core.Errors;
using Quote.Domain.Core.Primitives.Maybe;
using Quote.Domain.Core.Primitives.Result;

namespace Quote.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class QuoteController(IMediator mediator):ApiController(mediator)
{
    [HttpGet()]
    [ProducesResponseType(typeof(PagedList<QuoteResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllQuotesQuery request) =>
        await Maybe<GetAllQuotesQuery>
            .From(request)
            .Bind(query => Mediator.Send(query, HttpContext.RequestAborted))
            .Match(Ok, NotFound);
    
    [HttpGet(ApiRoutes.Quotes.GetById)]
    [ProducesResponseType(typeof(QuoteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<IActionResult> GetById(Guid QuoteId) =>
        await Maybe<GetQuoteByIdQuery>
            .From(new GetQuoteByIdQuery(QuoteId))
            .Bind(query => Mediator.Send(query, HttpContext.RequestAborted))
            .Match(Ok, NotFound);

    [HttpPost(ApiRoutes.Quotes.Create)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> Create(CreateQuoteRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(request => new CreateQuoteCommand(
                request.Author,
                request.Text,
                request.CategoryId))
            .Bind(command => Mediator.Send(command, HttpContext.RequestAborted))
            .Match(Ok, BadRequest);

    [HttpPut(ApiRoutes.Quotes.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> Update(Guid quoteId, UpdateQuoteRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(request => new UpdateQuoteCommand(
                quoteId,
                request.Author,
                request.Text,
                request.CategoryId))
            .Bind(command => Mediator.Send(command, HttpContext.RequestAborted))
            .Match(Ok, BadRequest);

    [HttpDelete(ApiRoutes.Quotes.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> Delete(Guid quoteId) =>
        await Result.Success(new DeleteQuoteCommand(quoteId))
            .Bind(command => Mediator.Send(command, HttpContext.RequestAborted))
            .Match(Ok, BadRequest);
    
    [HttpGet(ApiRoutes.Quotes.Random)]
    [ProducesResponseType(typeof(QuoteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<IActionResult> GetRandom() =>
        await Maybe<GetRandomQuoteQuery>
            .From(new GetRandomQuoteQuery())
            .Bind(query => Mediator.Send(query, HttpContext.RequestAborted))
            .Match(Ok, NotFound);
    
   
}