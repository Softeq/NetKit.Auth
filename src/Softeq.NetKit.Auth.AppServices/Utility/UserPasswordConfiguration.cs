// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.Auth.AppServices.Utility
{
    public class UserPasswordConfiguration
    {
        public int RequiredLength { get; set; }
        public int MaximumLength { get; set; }
        public string Regex { get; set; }
    }
}