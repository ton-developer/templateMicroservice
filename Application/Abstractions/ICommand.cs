using MediatR;

namespace Application.Abstractions;

public interface ICommand : IBaseCommand, IRequest
{
}

public interface ICommand<out TResponse> : IBaseCommand, IRequest<TResponse>
{
}

public interface IBaseCommand
{
}
