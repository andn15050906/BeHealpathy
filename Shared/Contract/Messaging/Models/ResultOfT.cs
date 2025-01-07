using Microsoft.AspNetCore.Mvc;

namespace Contract.Messaging.Models;

public class Result<T> : Result
{
    public T? Data { get; init; }

    public Result() { }

    public Result(short status) : base(status) { }

    public Result(short status, T data) : base(status)
    {
        Status = status;
        Data = data;
    }

    public Result(short status, string? message) : base(status, message)
    {
        Status = status;
        Message = message;
    }



    public override IActionResult AsResponse()
    {
        if (Data is null || Data is Guid guid && guid == default)
            return new JsonResult(Message) { StatusCode = Status };

        return new JsonResult(Data) { StatusCode = Status };
    }
}