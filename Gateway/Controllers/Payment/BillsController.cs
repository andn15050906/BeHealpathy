using Contract.Domain.UserAggregate.Constants;
using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Payment;
using Contract.Requests.Payment.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Payment;

public sealed class BillsController : ContractController
{
    public BillsController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet("search")]
    [Authorize(Roles = RoleConstants.ADMIN)]
    public async Task<IActionResult> Get([FromQuery] QueryBillDto dto)
    {
        GetPagedBillsQuery query = new(dto);
        return await Send(query);
    }

    /*[HttpGet]
    public async Task<IActionResult> RedirectedFromVNPay(
        [FromQuery] VNPayResponseDto response,
        [FromServices] IOptions<AppInfoOptions> appInfo)
    {
        var clientUrl = appInfo.Value.MainFrontendApp;
        if (response is null)
        {
            //_logger.Warn("Null vnpResponse" + JsonSerializer.Serialize(vnpResponse));
            return Redirect(clientUrl + "/404");
        }
        var guids = TextHelper.GetGuidsFromString(response.vnp_OrderInfo, 1);
        if (guids.Count == 0 || guids[0] == default)
        {
            //_logger.Warn("Null client");
            return Redirect(clientUrl + "/404");
        }
        Guid clientId = guids[0];
        List<Guid> identifiers = TextHelper.GetGuidsFromString(response.vnp_OrderInfo, 2);
        if (identifiers.Count < 2)
        {
            //_logger.Warn("Invalid identifiers" + dto.vnp_OrderInfo);
            return Redirect(clientUrl + $"/404");
        }
        Guid client = identifiers[0];
        Guid courseId = identifiers[1];
        if (string.IsNullOrWhiteSpace(response.vnp_BankTranNo))
        {
            //_logger.Warn("Failed" + dto.vnp_BankTranNo);
            return Redirect(clientUrl + $"/Payment?courseId={courseId}&failed=true");
        }

        CreateBillDto billDto = new()
        {
            Action = BusinessMessages.Payment.ACTION_PAY_COURSE,
            Note = response.vnp_OrderInfo,
            Gateway = BusinessMessages.Payment.GATEWAY_VNPAY,

            Amount = response.vnp_Amount,
            TransactionId = response.vnp_TransactionNo,
            ClientTransactionId = response.vnp_BankTranNo,
            Token = string.Empty,
            IsSuccessful = true
        };
        CreateBillCommand command

        try
        {
            var createBillResult = Send()

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
    }*/

    /*[HttpGet("redirect-link")]
    [Authorize]
    public async Task<IActionResult> GetRedirectLink([FromQuery] CreateBillDto dto)
    {
        int amount = 0;
        string orderInfo = string.Empty;

        switch (dto.Action)
        {
            case PaymentDomainMessages.ACTION_PAY_COURSE:
                if (!Guid.TryParse(dto.Note, out var courseId))
                    return BadRequest(PaymentDomainMessages.INVALID_NOTE);
                var courseResult = await courseService.GetMinAsync(courseId);
                if (!courseResult.IsSuccessful)
                    return BadRequest(PaymentDomainMessages.INVALID_NOTE);

                var course = courseResult.Data!;
                amount = CourseBusinessHelper.GetPostDiscount(course.Price, course.Discount, course.DiscountExpiry);
                orderInfo = $"{client}'s payment for course #{courseId}";
                break;
            default:
                return BadRequest(PaymentDomainMessages.INVALID_ACTION);
        }

        var request = new VNPayHelper.VNPayRequest
        {
            vnp_Amount = amount,
            vnp_OrderInfo = orderInfo,
            vnp_ReturnUrl = $"{appInfo.Value.MainBackendApp}/api/bills"
            //vnp_IpAddr = HttpContext.GetClientIPAddress()
        };
        string url = new VNPayHelper().BuildPaymentUrl(request);

        return Ok(url);
    }*/
}
