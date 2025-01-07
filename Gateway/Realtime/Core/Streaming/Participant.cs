using Gateway.Realtime.Core.Streaming.Dtos;

namespace Gateway.Realtime.Core.Streaming;

public sealed class Participant
{
    // Can be anonymous
    public Guid? UserId { get; init; }
    public string ConnectionId { get; init; }
    public string? FullName { get; init; }
    public string? Avatar { get; init; }
    public string? RoomId { get; private set; }

    public Participant(Guid? userId, string connectionId, ParticipantExtraInfo info)
    {
        UserId = userId;
        ConnectionId = connectionId;
        FullName = info.FullName;
        Avatar = info.Avatar;
    }

    public void SetRoom(string roomId)
    {
        RoomId = roomId;
    }
}
