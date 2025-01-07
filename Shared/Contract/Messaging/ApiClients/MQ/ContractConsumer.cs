using Contract.Messaging.Models;
using MassTransit;

namespace Contract.Messaging.ApiClients.MQ;

public abstract class ContractConsumer<TContext, TRequest> : IConsumer<TRequest>
    where TContext : class
    where TRequest : class
{
    protected readonly TContext _context;
    protected readonly IMediator _mediator;

    public ContractConsumer(TContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public abstract Task Consume(ConsumeContext<TRequest> context);

    protected async Task<Result<T>> Send<T>(IRequest<Result<T>> request)
    {
        try
        {
            return await _mediator.Send(request);
        }
        catch (Exception ex)
        {
            return new Result<T>(500, ex.Message);
        }
    }

    protected async Task<Result> Send(IRequest<Result> request)
    {
        try
        {
            return await _mediator.Send(request);
        }
        catch (Exception ex)
        {
            return new Result(500, ex.Message);
        }
    }
}