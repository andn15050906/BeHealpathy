using Contract.Helpers;
using Contract.Messaging.Models;

namespace Contract.Messaging.CQRS;

public abstract class RequestHandler<TRequest, TContext> : IRequestHandler<TRequest, Result>
    where TRequest : class, IRequest<Result>
    where TContext : class
{
    protected readonly TContext _context;
    protected readonly IAppLogger _logger;

    public RequestHandler(TContext context, IAppLogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public abstract Task<Result> Handle(TRequest request, CancellationToken cancellationToken);






    protected static Result Ok() => new(200);

    protected static Result Created() => new(201);

    protected static Result BadRequest(string message) => new(400, message);

    protected static Result Unauthorized(string message) => new(401, message);

    protected static Result Forbidden(string message) => new(403, message);

    protected static Result NotFound(string message) => new(404, message);

    protected static Result Conflict(string message) => new(409, message);

    protected static Result ServerError(string message) => new(500, message);
}