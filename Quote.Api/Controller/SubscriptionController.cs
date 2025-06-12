using MediatR;
using Microsoft.AspNetCore.Mvc;
using Quote.Api.Contracts;
using Quote.Application.Subscribers.Commands.CreateSubscription;
using Quote.Application.Subscribers.Commands.RemoveSubscriptionCommand;
using Quote.Application.Subscribers.Queries.GetActiveSubscribers;
using Quote.Contracts.Requests.SubscriberRequest;
using Quote.Domain.Core.Primitives.Maybe;

namespace Quote.Api.Controller;

[ApiController]
[Route("[controller]")]
public class SubscriptionController(IMediator mediator) : ApiController(mediator)
{
    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] CreateSubscriberRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateSubscriptionCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.TelegramUser);

        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
            return Ok(new { SubscriberId = result.Value, Message = "Successfully subscribed!" });

        return BadRequest(result.Error);
    }

    [HttpDelete("{subscriberId:guid}")]
    public async Task<IActionResult> Unsubscribe(RemoveSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var command = new RemoveSubscriptionCommand(request.FirstName,request.LastName,request.Email,request.TelegramUser);
        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
            return Ok(new { Message = "Successfully unsubscribed!" });

        return BadRequest(result.Error);
    }
    
    [HttpGet("active")]
    public async Task<IActionResult> GetActiveSubscribers([FromQuery] GetActiveSubscribersQuery request) =>
        await Maybe<GetActiveSubscribersQuery>
            .From(request)
            .Bind(command => Mediator.Send(command))
            .Match(result => Ok(result), BadRequest);
    

    
}