using Contract.Helpers;
using Contract.Helpers.Storage;
using Infrastructure.DataAccess.SQLServer.Helpers;
using Contract.Requests.Identity.UserRequests.Dtos;
using Contract.Requests.Identity.UserRequests;

namespace Gateway.Services.Identity.UserHandlers;

public class UpdateUserHandler : RequestHandler<UpdateUserCommand, HealpathyContext>
{
    private readonly IFileService _fileService;

    public UpdateUserHandler(HealpathyContext context, IAppLogger logger, IFileService fileService) : base(context, logger)
    {
        _fileService = fileService;
    }



    public override async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId == default)
            return Unauthorized(string.Empty);
        User? entity = await _context.Users.FindExt(request.UserId);
        if (entity is null)
            return Unauthorized(string.Empty);

        if (request.Rq.CurrentPassword is not null)
        {
            if (request.Rq.NewPassword is null)
                return BadRequest(BusinessMessages.User.INVALID_NEWPASSWORD_MISSING);
            if (!User.IsMatchPasswords(request.Rq.CurrentPassword, entity.Password))
                return Unauthorized(BusinessMessages.User.UNAUTHORIZED_PASSWORD);
        }

        try
        {
            await ApplyChanges(request.Rq, entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(string.Empty);
        }
    }

    private async Task ApplyChanges(UpdateUserDto dto, User entity)
    {
        if (dto.FullName is not null)
            entity.SetFullName(dto.FullName);
        if (dto.Avatar is not null)
        {
            // should've been Multimedia
            entity.AvatarUrl = (await _fileService.ReplaceImage(
                await MediaWithStream.FromImageDto(dto.Avatar, entity.Id, _ => _fileService.GetUserAvatarIdentifier(entity.Id)),
                entity.AvatarUrl
            ))?.Url ?? string.Empty;
        }
        if (dto.DateOfBirth is not null)
            entity.DateOfBirth = (DateTime)dto.DateOfBirth;
        if (dto.Bio is not null)
            entity.Bio = dto.Bio;

        if (dto.CurrentPassword is not null && dto.NewPassword is not null)
            entity.SetPassword(dto.NewPassword);
        if (dto.RoadmapId is not null)
            entity.RoadmapId = dto.RoadmapId;
    }
}
