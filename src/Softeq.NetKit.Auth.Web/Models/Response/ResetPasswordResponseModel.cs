// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.Auth.Web.Models.Response
{
    public class ResetPasswordResponseModel
    {      
        public ResetPasswordResponseModel(bool isSuccessful, string errorMessage)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
        }

        public bool IsSuccessful { get; }
        public string ErrorMessage { get; }
    }
}