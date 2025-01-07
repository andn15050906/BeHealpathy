using Microsoft.AspNetCore.Mvc;

namespace Contract.Messaging.Models;

public class Result
{
    public short Status { get; init; }
    public string? Message { get; init; }
    public bool IsSuccessful { get => Status < 300; }

    public Result()
    {

    }

    public Result(short status)
    {
        Status = status;
    }

    public Result(short status, string? message)
    {
        Status = status;
        Message = message;
    }



    public virtual IActionResult AsResponse()
    {
        return new JsonResult(Message) { StatusCode = Status };
    }
}