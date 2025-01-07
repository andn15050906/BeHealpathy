namespace Contract.BusinessRules.Messaging;

public partial class BusinessMessages
{
    public struct Instructor
    {
        // 400
        public const string INVALID_BALANCE = "400: Invalid balance amount";
    }

    public struct Category
    {
        // 400
        public const string INVALID_ID = "400: Invalid id";
        public const string INVALID_PARENT = "400: Invalid parent";
    }

    public struct Course
    {
        // 400
        public const string INVALID_CATEGORY = "400: Invalid category";
        public const string INVALID_PARENT = "400: Invalid parent";
        public const string INVALID_USER = "400: Invalid user";
        public const string INVALID_DISCOUNT = "400: Discount must be between 0 and 100%";
        public const string INVALID_DISCOUNT_EXPIRY = "400: Invalid discount expiration date";
        public const string INVALID_SECTION = "400: Invalid section";
        public const string INVALID_LECTURE = "400: Invalid lecture";
        public const string INVALID_RATING = "400: The rating must be between 1 and 5";
        public const string INVALID_INSTRUCTOR = "400: Invalid instructor";

        // 500
        public const string public_BAD_MILESTONES = "400: Invalid milestones";
    }

    public struct Enrollment
    {
        // 400
        public const string INVALID_BILL_OR_GRANTED = "400: The course must be either purchased or granted";
    }

    public struct CourseReview
    {
        public const string INVALID_RATING = "400: The rating must be between 1 and 5";
    }
}
