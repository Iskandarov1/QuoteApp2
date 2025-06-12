using MediatR;

namespace Quote.Application.Core.Abstractions.Messaging
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}