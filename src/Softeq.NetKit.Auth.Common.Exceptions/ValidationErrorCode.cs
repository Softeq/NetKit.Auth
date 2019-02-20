// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.Auth.Common.Exceptions
{
    public static class ValidationErrorCode
    {
        public static string MaxLengthExceeded => "max_length_exceeded";
        public static string MinLengthRequired => "min_length_required";
        public static string FieldIsEmpty => "field_is_empty";
        public static string InvalidEmail => "invalid_email";
        public static string WeakPassword => "weak_password";
        public static string ConfirmPasswordDoesNotMatch => "confirm_password_does_not_match";
    }
}