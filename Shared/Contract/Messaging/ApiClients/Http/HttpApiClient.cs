using Contract.Messaging.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Contract.Messaging.ApiClients.Http;

public abstract class HttpApiClient : BaseApiClient
{
    protected readonly HttpClient _apiClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public HttpApiClient(HttpClient httpClient)
    {
        _apiClient = httpClient;
        _jsonOptions = new() { PropertyNameCaseInsensitive = true };
    }

    public async Task<Result<T>> GetAsync<T>(string? requestUri)
    {
        try
        {
            return await ReadJsonAsResult<T>(await _apiClient.GetAsync(requestUri));
        }
        catch
        {
            return new Result<T>(500);
        }
    }

    public async Task<Result<TResponse>> PostAsync<T, TResponse>(string? requestUri, T content)
    {
        var jContent = JsonContent.Create(content);

        try
        {
            return await ReadJsonAsResult<TResponse>(await _apiClient.PostAsync(requestUri, jContent));
        }
        catch
        {
            return new Result<TResponse>(500);
        }
    }

    public async Task<Result> PostAsync<T>(string? requestUri, T content)
    {
        var jContent = JsonContent.Create(content);

        try
        {
            return await ReadAsResult(await _apiClient.PostAsync(requestUri, jContent));
        }
        catch
        {
            return new Result(500);
        }
    }

    public async Task<Result> PatchAsync<T>(string? requestUri, T content)
    {
        var jContent = JsonContent.Create(content);

        try
        {
            return await ReadAsResult(await _apiClient.PatchAsync(requestUri, jContent));
        }
        catch
        {
            return new Result(500);
        }
    }

    public async Task<Result> DeleteAsync(string? requestUri)
    {
        try
        {
            return await ReadAsResult(await _apiClient.DeleteAsync(requestUri));
        }
        catch
        {
            return new Result(500);
        }
    }






    private async Task<Result<T>> ReadJsonAsResult<T>(HttpResponseMessage response)
    {
        short status = (short)response.StatusCode;

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
            if (responseContent is null)
                return new Result<T>(500);
            return new Result<T>(status, responseContent);
        }
        return new Result<T>(status, await response.Content.ReadAsStringAsync());
    }

    private async Task<Result> ReadAsResult(HttpResponseMessage response)
    {
        short status = (short)response.StatusCode;

        var responseContent = await response.Content.ReadAsStringAsync();
        return new Result(status, responseContent);
    }
}
