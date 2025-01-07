using Contract.Requests.Payment;

namespace Gateway.Services.Payment;

/*public sealed class CreateBillHandler : RequestHandler<CreateBillCommand, HealpathyContext>
{
    public CreateBillHandler(HealpathyContext context) : base(context) { }



    public override Task<Result> Handle(CreateBillCommand command, CancellationToken cancellationToken)
    {

        var clientUrl = appInfo.Value.MainFrontendApp;

        if (vnpResponse is null)
        {
            _logger.Warn("Null vnpResponse" + JsonSerializer.Serialize(vnpResponse));
            return Redirect(clientUrl + "/404");
        }

        //var clientId = HttpContext.GetClientId();
        var guids = TextHelper.GetGuidsFromString(vnpResponse.vnp_OrderInfo, 1);
        if (guids.Count == 0 || guids[0] == default)
        {
            _logger.Warn("Null client");
            return Redirect(clientUrl + "/404");
        }
        Guid clientId = guids[0];

        List<Guid> identifiers = TextHelper.GetGuidsFromString(vnpResponse.vnp_OrderInfo, 2);
        if (identifiers.Count < 2)
        {
            _logger.Warn("Invalid identifiers" + vnpResponse.vnp_OrderInfo);
            return Redirect(clientUrl + $"/404");
        }
        Guid client = identifiers[0];
        Guid courseId = identifiers[1];

        if (string.IsNullOrEmpty(vnpResponse.vnp_BankTranNo))
        {
            _logger.Warn("Failed" + vnpResponse.vnp_BankTranNo);
            return Redirect(clientUrl + $"/Payment?courseId={courseId}&failed=true");
        }

        Guid billId = Guid.NewGuid();
        CreateBillDto dto = new()
        {
            Action = PaymentDomainMessages.ACTION_PAY_COURSE,
            Note = vnpResponse.vnp_OrderInfo,
            Gateway = PaymentDomainMessages.GATEWAY_VNPAY
        };
        PaymentResponse paymentResponse = new()
        {
            Amount = vnpResponse.vnp_Amount,
            TransactionId = vnpResponse.vnp_TransactionNo,
            ClientTransactionId = vnpResponse.vnp_BankTranNo,
            Token = string.Empty,
            IsSuccessful = true
        };

        try
        {
            var billTask = billService.Create(billId, dto, paymentResponse, clientId);
            var enrollmentTask = enrollmentService.Enroll(courseId, client, billId);
            await Task.WhenAll(billTask, enrollmentTask);
            await enrollmentService.ForceCommitAsync();
            _logger.Warn("Succeed" + vnpResponse.vnp_BankTranNo);
            return Redirect(clientUrl + $"/Course/Detail?id={courseId}");
        }
        catch
        {
            _logger.Warn("Failed false");
            return Redirect(clientUrl + $"/Payment?courseId={courseId}&failed=false");
        }
    }
}
*/