using Contract.Domain.UserAggregate.Constants;
using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Payment;
using Contract.Requests.Payment.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;
using Contract.Helpers.AppExploration;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Contract.BusinessRules.PaymentBiz;

namespace Gateway.Controllers.Payment;

public sealed class BillsController : ContractController
{
    private readonly PayOS _payOS;

    public BillsController(IMediator mediator, PayOS payOS) : base(mediator)
    {
        _payOS = payOS;
    }

    [HttpGet("search")]
    [Authorize(Roles = RoleConstants.ADMIN)]
    public async Task<IActionResult> Get([FromQuery] QueryBillDto dto)
    {
        GetPagedBillsQuery query = new(dto);
        return await Send(query);
    }



    [HttpPost("purchase/premium/{option}")]
    [Authorize]
    public async Task<IActionResult> PurchasePremium(PaymentOptions option, [FromServices] IOptions<AppInfoOptions> appInfo)
    {
        long orderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var billId = Guid.NewGuid();
        var cancelUrl = $"{appInfo.Value.MainBackendApp}/api/bills/redirect-payos/premium/{billId}/cancelled";
        var returnUrl = $"{appInfo.Value.MainBackendApp}/api/bills/redirect-payos/premium/{billId}/success";

        var subscriptions = new List<ItemData>
        {
            new(PaymentOptions.YearlyPremium.ToString(), 1, 240000),
            new(PaymentOptions.MonthlyPremium.ToString(), 1, 25000)
        };
        var selectedSubscription = subscriptions[(int)option];

        var paymentData = new PaymentData(orderCode, selectedSubscription.price, selectedSubscription.name, [selectedSubscription], cancelUrl, returnUrl);
        var result = await CreatePaymentLinkAndBill(paymentData, billId, string.Empty);
        return Ok(new { url = result.checkoutUrl });
    }

    [HttpPost("purchase/course/{courseId}")]
    [Authorize]
    public async Task<IActionResult> PurchaseCourse(Guid courseId, [FromServices] HealpathyContext context, [FromServices] IOptions<AppInfoOptions> appInfo)
    {
        long orderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var billId = Guid.NewGuid();
        var cancelUrl = $"{appInfo.Value.MainBackendApp}/api/bills/redirect-payos/course/{billId}/cancelled";
        var returnUrl = $"{appInfo.Value.MainBackendApp}/api/bills/redirect-payos/course/{billId}/success";

        var course = await context.Courses.FirstOrDefaultAsync(_ => _.Id == courseId && !_.IsDeleted);
        if (course is null)
            return NotFound();
        var item = new ItemData(course.Title, 1, (int)course.Price);

        var paymentData = new PaymentData(orderCode, (int)course.Price, PaymentOptions.Enrollment.ToString(), [item], cancelUrl, returnUrl);
        var result = await CreatePaymentLinkAndBill(paymentData, billId, course.Id.ToString());
        return Ok(new { url = result.checkoutUrl });
    }

    private async Task<CreatePaymentResult> CreatePaymentLinkAndBill(PaymentData paymentData, Guid billId, string note)
    {
        var result = await _payOS.createPaymentLink(paymentData);
        var billCommand = new CreateBillCommand(
                billId,
                new CreateBillDto
                {
                    Action = paymentData.description,                             // PaymentOptions
                    Amount = paymentData.items[0].price,
                    ClientTransactionId = result.paymentLinkId.ToString(),
                    Gateway = "PayOS",
                    Note = note,
                    TransactionId = result.paymentLinkId.ToString()
                },
                ClientId
        );
        await _mediator.Send(billCommand);
        return result;
    }



    [HttpGet("redirect-payos/premium/{billId}/{status}")]
    public async Task<IActionResult> PremiumPurchased(
        Guid billId, string status, [FromQuery] PaymentResponseDto response,
        [FromServices] IOptions<AppInfoOptions> appInfo
    )
    {
        //...
        //status
        /*
        public string code { get; set; }
        public string id { get; set; }
        public bool cancel { get; set; }
        public string status { get; set; }
        public string orderCode { get; set; }
        */

        var feUrl = appInfo.Value.MainFrontendApp;
        string[] statuses = ["PAID", "PENDING", "PROCESSING", "CANCELLED"];

        if (response is null)
            return Redirect($"{feUrl}/courses");

        var command = new UpdateBillCommand(
            new UpdateBillDto
            {
                Id = billId,
                IsSuccessful = response.status == statuses[0]
            },
            ClientId
        );
        await _mediator.Send(command);
        return Redirect($"{feUrl}/profile");
    }

    [HttpGet("redirect-payos/course/{billId}/{status}")]
    public async Task<IActionResult> CoursePurchased(
        Guid billId, string status, [FromQuery] PaymentResponseDto response,
        [FromServices] IOptions<AppInfoOptions> appInfo
    )
    {
        //...
        //status
        /*
        public string code { get; set; }
        public string id { get; set; }
        public bool cancel { get; set; }
        public string status { get; set; }
        public string orderCode { get; set; }
        */

        var feUrl = appInfo.Value.MainFrontendApp;
        string[] statuses = ["PAID", "PENDING", "PROCESSING", "CANCELLED"];

        if (response is null)
            return Redirect($"{feUrl}/courses");

        var command = new UpdateBillCommand(
            new UpdateBillDto
            {
                Id = billId,
                IsSuccessful = response.status == statuses[0]
            },
            ClientId
        );
        var result = await _mediator.Send(command);
        if (result.IsSuccessful)
            return Redirect($"{feUrl}/courses/{result.Data}");
        return Redirect($"{feUrl}/courses");
    }
}