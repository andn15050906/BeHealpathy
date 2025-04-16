using Contract.Domain.Shared;
using Contract.Helpers;
using Contract.Requests.Payment;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Payment;

public sealed class CreateBillHandler(HealpathyContext context, IAppLogger logger, IEventCache cache)
    : RequestHandler<CreateBillCommand, HealpathyContext>(context, logger, cache)
{
    public override async Task<Result> Handle(CreateBillCommand command, CancellationToken cancellationToken)
    {
        var entity = Adapt(command);

        try
        {
            await _context.Bills.InsertExt(entity);
            await _context.SaveChangesAsync();

            _cache.Add(command.UserId, new Events.BillCreated(entity.Id));
            return Created();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private static Bill Adapt(CreateBillCommand command)
    {
        return new Bill(
            command.Id, command.UserId,
            command.Rq.Action, command.Rq.Note, command.Rq.Amount, command.Rq.Gateway,
            command.Rq.TransactionId, command.Rq.ClientTransactionId, command.Rq.Token, command.Rq.IsSuccessful
        );
    }
}