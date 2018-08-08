namespace TravelPlanner.BusinessLogic.Models
{
    public static class ValidationResultCodes
    {
        public const string LoginWrongCredentials = "loginWrongCredentials";
        public const string UserNameRequiredOrInvalid = "userNameRequiredOrInvalid";

        public const string EmailRequiredOrInvalid = "emailRequiredOrInvalid";
        public const string DuplicateEmail = "duplicateEmail";

        public const string PasswordShort = "passwordShort";
        public const string PasswordInvalidFormat = "passwordInvalidFormat";

        public static string InviteNotFound = "inviteNotFound";
    }
}
