using Microsoft.AspNetCore.Mvc;

namespace Contract.Messaging.ApiClients.Http;

public class ContractController : BaseController
{
    protected readonly IMediator _mediator;

    public ContractController(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected async Task<IActionResult> Send<T>(IRequest<Result<T>> request)
        => (await _mediator.Send(request)).AsResponse();

    protected async Task<IActionResult> Send(IRequest<Result> request)
        => (await _mediator.Send(request)).AsResponse();
}