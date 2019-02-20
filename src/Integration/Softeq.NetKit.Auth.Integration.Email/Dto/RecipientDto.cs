// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.Auth.Integration.Email.Dto
{
    public class RecipientDto
    {
        public RecipientDto(string email, string name, EmailDeliveryType deliveryType = EmailDeliveryType.Regular)
        {
            Email = email;
            Name = name;
            DeliveryType = deliveryType;
        }

        public string Name { get; set; }

        public string Email { get; set; }

        public EmailDeliveryType DeliveryType { get; set; }
    }
}
