using Contract.Helpers;
using Contract.Services.Contracts;

namespace Contract.Messaging.CQRS;

public abstract class RequestHandler<TRequest, TResponse, TContext> : IRequestHandler<TRequest, Result<TResponse>>
    where TRequest : class, IRequest<Result<TResponse>>
    where TContext : class
{
    protected readonly TContext _context;
    protected readonly IAppLogger _logger;
    protected readonly IEventCache _cache;

#pragma warning disable CS8618
    public RequestHandler(TContext context, IAppLogger logger)
    {
        _context = context;
        _logger = logger;
    }
#pragma warning restore CS8618

    public RequestHandler(TContext context, IAppLogger logger, IEventCache cache)
    {
        _context = context;
        _logger = logger;
        _cache = cache;
    }

    public abstract Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);






    protected static Result<TResponse> Ok(TResponse response) => new(200, response);

    protected static Result<TResponse> Created() => new(201);
    protected static Result<TResponse> Created(TResponse response) => new(201, response);

    protected static Result<TResponse> BadRequest(string message) => new(400, message);

    protected static Result<TResponse> Unauthorized(string message) => new(401, message);

    protected static Result<TResponse> Forbidden(string message) => new(403, message);

    protected static Result<TResponse> NotFound(string message) => new(404, message);

    protected static Result<TResponse> Conflict(string message) => new(409, message);

    protected static Result<TResponse> ServerError(string message) => new(500, message);






    protected static Result<PagedResult<T>> ToQueryResult<T>(PagedResult<T> pagedList)
    {
        if (pagedList is null || pagedList.TotalCount == 0)
            return new(404, string.Empty);
        return new(200, pagedList);
    }

    protected static Result<IEnumerable<T>> ToQueryResult<T>(IEnumerable<T> values)
    {
        if (values is null || !values.Any())
            return new(404, string.Empty);
        return new(200, values);
    }

    protected static Result<T> ToQueryResult<T>(T? value)
    {
        return value is null ? new(404, string.Empty) : new(200, value);
    }
}