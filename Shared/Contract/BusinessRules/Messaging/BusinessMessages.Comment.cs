namespace Contract.BusinessRules.Messaging;

public partial class BusinessMessages
{
    public struct Comment
    {
        // 400
        public const string INVALID_TARGET_ENTITY = "400: Invalid Target Entity. Target Entity should be 0 or 1.";
    }
}
