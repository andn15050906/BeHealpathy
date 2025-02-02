using Gateway.Realtime.Core.Streaming;
using Microsoft.AspNetCore.SignalR;

namespace Gateway.Realtime.Core;

public sealed class ConnectionsHandler
{
    private static readonly Dictionary<string, Guid?> _connection_user = [];

    private static readonly Dictionary<string, Participant> _connection_participant = [];
    private static readonly List<Room> _rooms = [];






    public static void Connected(HubCallerContext context)
    {
        try
        {
            _connection_user.Add(
                context.ConnectionId,
                (context.UserIdentifier == null || !Guid.TryParse(context.UserIdentifier, out Guid clientId)) ? null : clientId
            );
        }
        catch (Exception) { }
    }

    public static void Disconnected(string connection)
    {
        _connection_user.Remove(connection);
    }

    public static Guid? GetUserId(HubCallerContext context)
    {
        _connection_user.TryGetValue(context.ConnectionId, out Guid? userId);
        return userId;
    }






    public static Room? FindRoom(string roomId) => _rooms.Find(_ => _.Id == roomId);

    public static Participant? FindParticipant(string connectionId)
    {
        if (_connection_participant.TryGetValue(connectionId, out Participant? participant))
            return participant;
        return null;
    }

    public static Room CreateRoom(string roomId, Participant host)
    {
        //Room room = new (GenerateRoomId(), host);
        Room room = new(roomId, host);
        _rooms.Add(room);
        host.SetRoom(roomId);
        _connection_participant.Add(host.ConnectionId, host);
        return room;
    }

    public static bool TryAddRoomParticipant(Participant guest, Room room)
    {
        if (!room.TryAddParticipant(guest))
            return false;
        guest.SetRoom(room.Id);
        _connection_participant.Add(guest.ConnectionId, guest);
        return true;
    }

    public static void RemoveRoomParticipant(Room room, string guestConnectionId)
    {
        room.RemoveParticipant(guestConnectionId);
        _connection_participant.Remove(guestConnectionId);

        if (room.IsEmpty())
            _rooms.Remove(room);
    }






    public static Dictionary<string, Participant> GetParticipants() => _connection_participant;

    public static List<Room> GetRooms() => _rooms;
}
