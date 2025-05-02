using Microsoft.AspNetCore.SignalR;
using Gateway.Realtime.Core;
using Gateway.Realtime.Core.Streaming;
using Gateway.Realtime.Core.Streaming.Dtos;

namespace WisNet.Gateway.Realtime.Interface;

public sealed partial class AppHub
{
    public void SendOffers(OfferMessage[] offers, bool isRenegotiation)
    {
        Parallel.ForEach(offers, offer =>
        {
            Clients.Client(offer.Peer).SendAsync(StreamEvents.OfferReceived.ToString(), Context.ConnectionId, offer.Offer, isRenegotiation);
        });
    }

    public async Task SendAnswer(string offerer, string answer)
    {
        await Clients.Client(offerer).SendAsync(StreamEvents.AnswerReceived.ToString(), Context.ConnectionId, answer);
    }

    public async Task SendICECandidate(string receiver, string candidate)
    {
        await Clients.Client(receiver).SendAsync(StreamEvents.ICECandidateReceived.ToString(), Context.ConnectionId, candidate);
    }





    public async Task JoinRoom(string roomId, ParticipantExtraInfo info)
    {
        var room = ConnectionsHandler.FindRoom(roomId);
        var clientId = ConnectionsHandler.GetUserId(Context);
        var participant = new Participant(clientId, Context.ConnectionId, info);

        if (room == null)
        {
            ConnectionsHandler.CreateRoom(roomId, participant);
#pragma warning disable CS4014
            Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            Clients.Caller.SendAsync(StreamEvents.RoomCreated.ToString(), roomId);
#pragma warning restore CS4014
            return;
        }

        var status = ConnectionsHandler.TryAddRoomParticipant(participant, room);
        if (status.Item1 != 0)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

#pragma warning disable CS4014
            Clients.Caller.SendAsync(StreamEvents.RoomInfoReceived.ToString(), room);

            if (status.Item1 == 1 && !string.IsNullOrEmpty(status.Item2))
                Clients.OthersInGroup(roomId).SendAsync(StreamEvents.ParticipantLeft.ToString(), roomId, status.Item2);
            Clients.OthersInGroup(roomId).SendAsync(StreamEvents.RoomJoined.ToString(), roomId, participant);
#pragma warning restore CS4014
        }
    }

    public async Task SendRoom(string roomId, StreamMessage message/*, [FromServices] ChatService chatService*/)
    {
        var room = ConnectionsHandler.FindRoom(roomId);
        var participant = ConnectionsHandler.FindParticipant(Context.ConnectionId);

        if (room is null || participant is null)
            return;
        if (message.Event == StreamEvents.VideoOff.ToString())
        {
            await Clients.OthersInGroup(roomId).SendAsync(StreamEvents.RoomMessageReceived.ToString(), roomId, Context.ConnectionId, message);
        }
    }

    public async Task LeaveRoom(string roomId)
    {
        Room? room = ConnectionsHandler.FindRoom(roomId);
        if (room == null)
            return;
        ConnectionsHandler.RemoveRoomParticipant(room, Context.ConnectionId);
        await Clients.OthersInGroup(room.Id).SendAsync(StreamEvents.ParticipantLeft.ToString(), room.Id, Context.ConnectionId);
    }
}