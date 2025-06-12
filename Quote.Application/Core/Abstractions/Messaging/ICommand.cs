using MediatR;

namespace Quote.Application.Core.Abstractions.Messaging;


public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
