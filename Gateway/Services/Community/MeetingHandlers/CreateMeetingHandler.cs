using Contract.Domain.CommunityAggregate;
using Contract.Helpers;
using Contract.Requests.Community.MeetingRequests;
using Contract.Responses.Community;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Community.MeetingHandlers;

public sealed class CreateMeetingHandler : RequestHandler<CreateMeetingCommand, MeetingModel, HealpathyContext>
{
    public CreateMeetingHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<MeetingModel>> Handle(CreateMeetingCommand command, CancellationToken cancellationToken)
    {
        Meeting entity = Adapt(command);

        try
        {
            await _context.Meetings.InsertExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Created(MeetingModel.MapFunc(entity));
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private Meeting Adapt(CreateMeetingCommand command)
    {
        var id = Guid.NewGuid();
        var participants = command.Rq.Participants.Select(_ => new MeetingParticipant(_.UserId, id, _.IsHost)).ToList();

        return new Meeting(
            command.Id, command.UserId, command.Rq.Title, command.Rq.Description,
            command.Rq.StartAt, command.Rq.EndAt, command.Rq.MaxParticipants, participants);
    }
}