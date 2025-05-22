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
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml;
using Contract.Requests.Identity.UserRequests;
using Contract.Requests.Identity.UserRequests.Dtos;

namespace Gateway.Controllers.Shared;

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
        if (course is null || course.Price is null || course.Price == 0)
            return NotFound();
        double finalPrice = (double)course.Price;
        if (course.DiscountExpiry <= DateTime.UtcNow)
        {
            finalPrice = (double)course.Price * (1 - (course.Discount is null ? 0 : (double)course.Discount));
        }
        var item = new ItemData(course.Title, 1, (int)Math.Ceiling(finalPrice));

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
        Guid billId, string status, [FromQuery] PaymentResponseDto? response,
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
                IsSuccessful = (response.status ?? string.Empty).Equals(statuses[0], StringComparison.CurrentCultureIgnoreCase)
            },
            ClientId
        );
        await _mediator.Send(command);
        return Redirect($"{feUrl}/profile");
    }

    [HttpGet("redirect-payos/course/{billId}/{status}")]
    public async Task<IActionResult> CoursePurchased(
        Guid billId, string status, [FromQuery] PaymentResponseDto? response,
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
                IsSuccessful = (response.status ?? string.Empty).Equals(statuses[0], StringComparison.CurrentCultureIgnoreCase)
            },
            ClientId
        );
        var result = await _mediator.Send(command);
        if (result.IsSuccessful)
            return Redirect($"{feUrl}/courses/{result.Data}");
        return Redirect($"{feUrl}/courses");
    }



    [HttpGet("report")]
    [Authorize(Roles = RoleConstants.ADMIN)]
    public async Task<IActionResult> ExportReport()
    {
        var billResult = await _mediator.Send(new GetPagedBillsQuery(new QueryBillDto()));

        if (billResult.IsSuccessful)
        {
            var bills = billResult.Data!.Items;
            var userResult = await _mediator.Send(new GetPagedUsersQuery(new QueryUserDto()));
            var users = userResult.Data!.Items;

            var totalAmount = bills.Sum(b => b.Amount);
            var billsByGateway = bills.GroupBy(b => b.Gateway)
                                      .Select(g => new { Gateway = g.Key, Count = g.Count(), TotalAmount = g.Sum(b => b.Amount) })
                                      .ToList();
            var enrollmentBills = bills.Where(_ => _.Action == "Enrollment");
            var premiumBills = bills.Where(_ => _.Action.Contains("Premium"));

            var ms = new MemoryStream();
            using (var package = new ExcelPackage(ms))
            {
                var worksheet = package.Workbook.Worksheets.Add("Bills Data");

                worksheet.Cells[1, 1].Value = "#No.";
                worksheet.Cells[1, 2].Value = "Source of Payment";
                worksheet.Cells[1, 3].Value = "Additional Note";
                worksheet.Cells[1, 4].Value = "Amount";
                worksheet.Cells[1, 5].Value = "Gateway";
                worksheet.Cells[1, 6].Value = "Transaction Time";
                worksheet.Cells[1, 7].Value = "Bill Creator";

                int row = 2;
                foreach (var bill in bills)
                {
                    worksheet.Cells[row, 1].Value = row - 1;
                    worksheet.Cells[row, 2].Value = bill.Action;
                    worksheet.Cells[row, 3].Value = bill.Note;
                    worksheet.Cells[row, 4].Value = bill.Amount;
                    worksheet.Cells[row, 5].Value = bill.Gateway;
                    worksheet.Cells[row, 6].Value = bill.CreationTime;
                    worksheet.Cells[row, 7].Value = users.FirstOrDefault(_ => _.Id == bill.CreatorId)?.FullName ?? "System";
                    row++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var chart = worksheet.Drawings.AddChart("ActionChart", eChartType.Pie3D);
                chart.Title.Text = "Payment Source: From Enrollment and Premium";
                chart.SetPosition(3, 0, 10, 0);
                chart.SetSize(400, 300);

                var successCountCell = worksheet.Cells["H1"];
                var failureCountCell = worksheet.Cells["H2"];

                worksheet.Cells["N1"].Value = "Enrollment";
                worksheet.Cells["N2"].Value = "Premium";

                worksheet.Cells["O1"].Value = enrollmentBills.Count();
                worksheet.Cells["O2"].Value = premiumBills.Count();

                var successSeries = chart.Series.Add(worksheet.Cells["O1"], worksheet.Cells["N1"]);
                var failureSeries = chart.Series.Add(worksheet.Cells["O2"], worksheet.Cells["N2"]);

                var gatewayChart = worksheet.Drawings.AddChart("GatewayChart", eChartType.ColumnClustered);
                gatewayChart.Title.Text = "Bills Count by Gateway";
                gatewayChart.SetPosition(20, 0, 10, 0);
                gatewayChart.SetSize(600, 400);

                worksheet.Cells["Q1"].Value = "Gateway";
                worksheet.Cells["R1"].Value = "Count";

                int gatewayRow = 2;
                foreach (var g in billsByGateway)
                {
                    worksheet.Cells[$"Q{gatewayRow}"].Value = g.Gateway;
                    worksheet.Cells[$"R{gatewayRow}"].Value = g.Count;
                    gatewayRow++;
                }

                var gatewaysRange = worksheet.Cells[$"Q2:Q{gatewayRow - 1}"];
                var countsRange = worksheet.Cells[$"R2:R{gatewayRow - 1}"];

                var series = gatewayChart.Series.Add(countsRange, gatewaysRange);
                series.Header = "Bills Count";

                package.Save();
                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "report.xlsx");
            }
        }

        return NotFound();
    }
}