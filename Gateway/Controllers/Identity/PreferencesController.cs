using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Identity.PreferenceRequests;
using Contract.Requests.Identity.PreferenceRequests.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Identity;

public sealed class PreferencesController : ContractController
{
    public PreferencesController(IMediator mediator) : base(mediator) { }



    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetAll()
    {
        GetAllPreferenceSurveysQuery command = new();
        return await Send(command);
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> UpdatePreference(UpdatePreferenceDto dto)
    {
        UpdatePreferenceCommand command = new(dto, ClientId);
        return await Send(command);
    }
}