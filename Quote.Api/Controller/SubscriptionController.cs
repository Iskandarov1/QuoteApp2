using MediatR;
using Microsoft.AspNetCore.Mvc;
using Quote.Api.Contracts;
using Quote.Application.Subscribers.Commands.CreateSubscription;
using Quote.Application.Subscribers.Commands.RemoveSubscriptionCommand;
using Quote.Application.Subscribers.Queries.GetActiveSubscribers;
using Quote.Contracts.Requests.SubscriberRequest;
using Quote.Domain.Core.Errors;
using Quote.Domain.Core.Primitives.Maybe;
using Quote.Domain.Core.Primitives.Result;
using Quote.Domain.Repositories;

namespace Quote.Api.Controller;

[ApiController]
[Route("[controller]")]
public class SubscriptionController(IMediator mediator,IUniqueFileStorage storage) : ApiController(mediator)
{
    [HttpPost("subscribe")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> Subscribe([FromForm] CreateSubscriberRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(req => new CreateSubscriptionCommand(
                req.FirstName,
                req.LastName,
                req.Email,
                req.TelegramUser,
                req.AttachedFile is null ? null : storage.Save(req.AttachedFile)))                
            .Bind(cmd => Mediator.Send(cmd, HttpContext.RequestAborted))
            .Match(id => Ok(new                     
                    { SubscriberId = id, Message = "Successfully subscribed!" }), BadRequest); 

    
    [HttpDelete("{subscriberId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)] 
    public async Task<IActionResult> Unsubscribe(RemoveSubscriptionCommand request, CancellationToken cancellationToken) =>
        await Result.Success(new RemoveSubscriptionCommand(request.Email,request.TelegramUser))
            .Bind(cmd => Mediator.Send(cmd, HttpContext.RequestAborted))
            .Match(_ => Ok(new { Message = "Successfully unsubscribed!" }), BadRequest);

    
    [HttpGet("active")]
    public async Task<IActionResult> GetActiveSubscribers([FromQuery] GetActiveSubscribersQuery request) =>
        await Maybe<GetActiveSubscribersQuery>
            .From(request)
            .Bind(command => Mediator.Send(command))
            .Match(result => Ok(result), BadRequest);
    

    
}