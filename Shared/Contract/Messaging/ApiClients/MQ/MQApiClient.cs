using Contract.Messaging.Models;
using MassTransit;

namespace Contract.Messaging.ApiClients.MQ;

public abstract class MQApiClient : BaseApiClient
{
    protected readonly IScopedClientFactory _factory;

    public MQApiClient(IScopedClientFactory factory)
    {
        _factory = factory;
    }

    protected async Task<Result<TResponse>> Send<TRequest, TResponse>(TRequest request)
        where TRequest : class, IRequest<Result<TResponse>>
    {
        var client = _factory.CreateRequestClient<TRequest>();

        try
        {
            var response = await client.GetResponse<Result<TResponse>>(request);
            return response.Message;
        }
        catch (Exception ex)
        {
            return ServerError<TResponse>(ex.Message);
        }
    }

    protected async Task<Result> Send<TRequest>(TRequest request)
        where TRequest : class, IRequest<Result>
    {
        var client = _factory.CreateRequestClient<TRequest>();

        try
        {
            var response = await client.GetResponse<Result>(request);
            return response.Message;
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}