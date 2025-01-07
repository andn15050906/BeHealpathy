using Contract.Messaging.ApiClients.Http;
using Gateway.Realtime.Core;
using Gateway.Realtime.Core.Streaming;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Realtime.Interface;

// Not inherited from ContractController
public class RealtimeController : BaseController
{
    [HttpGet("participants")]
    public Dictionary<string, Participant> GetParticipants()
    {
        return ConnectionsHandler.GetParticipants();
    }

    [HttpGet("rooms")]
    public List<Room> GetRooms()
    {
        return ConnectionsHandler.GetRooms();
    }
}
