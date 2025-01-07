namespace Contract.BusinessRules.Messaging;

public partial class BusinessMessages
{
    public struct Payment
    {
        public const string ACTION_PAY_COURSE = "Pay for course";
        public const string GATEWAY_VNPAY = "VNPay";

        // 400
        public const string INVALID_ACTION = "400: Invalid action";
        public const string INVALID_NOTE = "400: Invalid note";
    }
}
