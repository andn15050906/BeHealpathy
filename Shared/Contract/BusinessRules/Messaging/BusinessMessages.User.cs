namespace Contract.BusinessRules.Messaging;

public partial class BusinessMessages
{
    public struct User
    {
        // Mostly 400
        public const string INVALID_EMAIL = "400: Invalid Email";
        public const string INVALID_USERNAME = "400: UserName must be between 6 and 20 characters";
        public const string INVALID_PASSWORD_6TO20CHARS = "400: Password must be between 6 and 20 characters";
        public const string INVALID_PASSWORD_REGEX = "400: Password must contain Uppercase, Lowercase and Number";
        public const string INVALID_PHONE = "400: Invalid phone number";
        public const string INVALID_BIO_LENGTH = "400: Invalid Bio length";
        public const string INVALID_NEWPASSWORD_MISSING = "400: New Password missing";
        public const string INVALID_RESETPASSWORD_ATTEMPT = "400: Invalid reset password attempt";
        public const string INVALID_EMAILPHONE_MISSING = "400: Must provide either email or phone";
        public const string INVALID_AMOUNT = "400: Invalid balance amount";

        //...
        public const string INVALID_SIGN_IN = "400: Invalid sign in";

        // 401
        public const string UNAUTHORIZED_PASSWORD = "401: Password does not match";
        public const string UNAUTHORIZED_SIGNIN = "401: Incorrect signin information";

        // 403
        public const string FORBIDDEN_FAILED_EXCEED = "403: You are forbidden to access";
        public const string FORBIDDEN_NOT_APPROVED = "403: You are not approved";

        // 404
        public const string NOT_FOUND = "User not found";

        // 409
        public const string CONFLICT_EMAIL = "This email has already been used";
        public const string CONFLICT_USERNAME = "This username has already been taken";
    }
}
