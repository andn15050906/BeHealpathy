using Contract.Messaging.Models;

namespace Contract.Messaging.ApiClients;

public abstract class BaseApiClient
{
    protected static Result Ok<T>() => new(200);
    protected static Result<T> Ok<T>(T result) => new(200, result);

    protected static Result Created() => new(201);
    protected static Result<T> Created<T>(T result) => new(201, result);

    protected static Result BadRequest(string message) => new(400, message);
    protected static Result<T> BadRequest<T>(string message) => new(400, message);

    protected static Result Unauthorized(string message) => new(401, message);
    protected static Result<T> Unauthorized<T>(string message) => new(401, message);

    protected static Result Forbidden(string message) => new(403, message);
    protected static Result<T> Forbidden<T>(string message) => new(403, message);

    protected static Result NotFound(string message) => new(404, message);
    protected static Result<T> NotFound<T>(string message) => new(404, message);

    protected static Result Conflict(string message) => new(409, message);
    protected static Result<T> Conflict<T>(string message) => new(409, message);

    protected static Result ServerError(string message) => new(500, message);
    protected static Result<T> ServerError<T>(string message) => new(500, message);
}