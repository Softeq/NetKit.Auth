// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.Auth.Integration.Email.Exceptions
{
    public class EmailException : System.Exception
    {
        public EmailException(string message)
            : base(message)
        {
        }

        public EmailException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
