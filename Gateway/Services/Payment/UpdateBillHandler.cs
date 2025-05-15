using Contract.BusinessRules.PaymentBiz;
using Contract.Domain.CourseAggregate;
using Contract.Domain.Shared;
using Contract.Helpers;
using Contract.Requests.Courses.EnrollmentRequests;
using Contract.Requests.Courses.EnrollmentRequests.Dtos;
using Contract.Requests.Payment;
using Core.Helpers;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Payment;

public sealed class UpdateBillHandler(HealpathyContext context, IAppLogger logger, IEventCache cache)
    : RequestHandler<UpdateBillCommand, Guid, HealpathyContext>(context, logger, cache)
{
    public override async Task<Result<Guid>> Handle(UpdateBillCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.Bills.FindExt(command.Rq.Id);
        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != command.UserId)
            return Unauthorized(string.Empty);
        var user = await _context.Users.FindExt(entity.CreatorId);
        if (user is null)
            return Unauthorized(string.Empty);

        var callbackEntityId = Guid.Empty;
        try
        {
            Adapt(entity, command);
            if (entity.IsSuccessful)
            {
                if (entity.Action == PaymentOptions.YearlyPremium.ToString())
                {
                    user.IsPremium = true;
                    user.PremiumExpiry ??= TimeHelper.Now;
                    user.PremiumExpiry = ((DateTime)user.PremiumExpiry).AddYears(1);
                }
                else if (entity.Action == PaymentOptions.MonthlyPremium.ToString())
                {
                    user.IsPremium = true;
                    user.PremiumExpiry ??= TimeHelper.Now;
                    user.PremiumExpiry = ((DateTime)user.PremiumExpiry).AddMonths(1);
                }
                else if (entity.Action == PaymentOptions.Enrollment.ToString())
                {
                    Guid.TryParse(entity.Note, out Guid courseId);
                    if (courseId != Guid.Empty)
                    {
                        CreateEnrollmentCommand enrollmentCommand = new(
                            Guid.NewGuid(),
                            new CreateEnrollmentDto { BillId = entity.Id, CourseId = courseId },
                            user.Id
                        );
                        await CreateEnrollment(enrollmentCommand);
                        callbackEntityId = courseId;
                    }
                }
            }

            await _context.SaveChangesAsync();
            return Ok(callbackEntityId);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private void Adapt(Bill entity, UpdateBillCommand command)
    {
        if (command.Rq.Action is not null)
            entity.Action = command.Rq.Action;
        if (command.Rq.Note is not null)
            entity.Note = command.Rq.Note;
        if (command.Rq.Amount is not null)
            entity.Amount = (long)command.Rq.Amount;
        if (command.Rq.TransactionId is not null)
            entity.TransactionId = command.Rq.TransactionId;
        if (command.Rq.Token is not null)
            entity.Token = command.Rq.Token;
        if (command.Rq.IsSuccessful is not null)
            entity.IsSuccessful = (bool)command.Rq.IsSuccessful;
    }






    // See CreateEnrollmentHandler
    public async Task<Result> CreateEnrollment(CreateEnrollmentCommand command)
    {
        try
        {
            if (command.Rq.BillId is null && !command.Rq.IsGranted)
                return BadRequest(BusinessMessages.Enrollment.INVALID_BILL_OR_GRANTED);

            var entity = Adapt(command);
            await _context.CourseProgress.InsertExt(entity);
            //await _context.SaveChangesAsync();

            _cache.Add(command.UserId, new Events.Course_Enrolled(command.Id));
            return Created();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private CourseProgress Adapt(CreateEnrollmentCommand command)
    {
        if (command.Rq.BillId is not null)
            return new CourseProgress(command.Id, command.Rq.CourseId, command.UserId, (Guid)command.Rq.BillId);
        return new CourseProgress(command.Id, command.Rq.CourseId, command.UserId);
    }
}