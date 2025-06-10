using MediatR;

namespace App.Application.Abstractions.Messaging;


public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
