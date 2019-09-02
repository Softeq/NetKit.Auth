// Developed by Softeq Development Corporation
// http://www.softeq.com
namespace Softeq.NetKit.Auth.Web.Utility
{
	public static class ConfigurationSettings
	{
		
		public const string AuthorizationHeaderName = "Authorization:Header";
		public const string AuthorizationKey = "Authorization:Key";

        public const string CertificateFileName = "SigningCertificate:FileName";
        public const string CertificatePassword = "SigningCertificate:Password";

        public const string AzureServiceBusCallAutoCancelQueueName = "AzureServiceBus:CallAutoCancelQueue";
		public const string AzureServiceBusConnectionString = "AzureServiceBus:ConnectionString";
		public const string AzureServiceBusMessageTimeToLive = "AzureServiceBus:MessageTimeToLive";
		public const string AzureServiceBusScope = "AzureServiceBus:Scope";
		public const string AzureServiceBusScheduledMessageEnqueueTime = "AzureServiceBus:ScheduledMessageEnqueueTime";
		public const string AzureServiceBusSubscriptionClientName = "AzureServiceBus:SubscriptionName";
		public const string AzureServiceBusTopicClientName = "AzureServiceBus:TopicName";

		public const string DatabaseConnectionString = "Database:ConnectionString";

		public const string DataProtectorProviderName = "DataProtectorTokenProvider:Options:Name";
		public const string DataProtectorProviderTokenLifespan = "DataProtectorTokenProvider:Options:TokenLifespan";

		public const string ApplicationUrl = "AuthApiUrlConfiguration:ApiUrl";
		public const string AuthResetPasswordPath = "AuthApiUrlConfiguration:ResetPasswordPath";
		public const string ConfirmEmailPath = "AuthApiUrlConfiguration:ConfirmEmailPath";

		public const string IdentityServerDefaultScheme = "IdentityServer:DefaultScheme";
		public const string IdentityServerLockoutMaxFailedAccessAttempts = "IdentityServer:Lockout:MaxFailedAccessAttempts";

		public const string SendGridSenderEmail = "SendGrid:SenderEmail";
		public const string SendGridSenderEmailName = "SendGrid:SenderEmailName";
		public const string SendGridApiKey = "SendGrid:APIKey";

		public const string SwaggerEndpointUrl = "Swagger:EndpointUrl";
		public const string SwaggerDescription = "Swagger:Description";
		public const string SwaggerName = "Swagger:Name";
		public const string SwaggerTitle = "Swagger:Title";
		public const string SwaggerVersion = "Swagger:Version";
		public const string SwaggerSchemeDescription = "Swagger:Scheme:Description";
		public const string SwaggerApiKeySchemeDescription = "Swagger:Scheme:ApiKeyDescription";
		public const string SwaggerSchemeName = "Swagger:Scheme:Name";
		public const string SwaggerSchemeIn = "Swagger:Scheme:In";
		public const string SwaggerSchemeType = "Swagger:Scheme:Type";
	
		public const string SerilogApplicationName = "Serilog:ApplicationName";
		public const string SerilogEnableLocalFileSink = "Serilog:EnableLocalFileSink";
		public const string SerilogFileSizeLimitMBytes = "Serilog:FileSizeLimitMBytes";

		public const string PasswordUniqueCount = "PasswordConfiguration:UniqueCount";
		public const string PasswordActivePeriodInDays = "PasswordConfiguration:ActivePeriodInDays";

	    public const string UserEmailMaximumLength = "UserEmailConfiguration:MaximumLength";

        public const string UserPasswordRequiredLength = "UserPasswordConfiguration:RequiredLength";
		public const string UserPasswordMaximumLength = "UserPasswordConfiguration:MaximumLength";
		public const string UserPasswordRegex = "UserPasswordConfiguration:Regex";
		
		public const string ApplicationInsightsInstrumentationKey = "ApplicationInsights:InstrumentationKey";
	    
	    public const string BaseEmailTemplate = "EmailTemplates:BaseEmail";
		public const string EmailConfirmationEmailTemplateName = "EmailTemplates:EmailConfirmationEmailTemplateName";
		public const string ResetPasswordEmailTemplateName = "EmailTemplates:ResetPasswordEmailTemplateName";
		public const string ChangePasswordEmailTemplateName = "EmailTemplates:ChangePasswordEmailTemplateName";
		public const string PasswordHasExpiredEmailTemplateName = "EmailTemplates:PasswordHasExpiredEmailTemplateName";
		public const string PasswordWillExpireEmailTemplate = "EmailTemplates:PasswordWillExpireEmail";
		public const string ServiceBusErrorEmail = "EmailTemplates:ServiceBusErrorEmail";

	    public const string MobileConfigurationIosConfirmEmailRedirectUrl = "MobileConfiguration:IosConfirmEmailRedirectUrl";

        public const string ClientId = "AppleHttpClientConfiguration:ClientId";
        public const string RedirectUri = "AppleHttpClientConfiguration:RedirectUri";
        public const string KeyId = "AppleHttpClientConfiguration:KeyId";
        public const string TeamId = "AppleHttpClientConfiguration:TeamId";
        public const string Lifetime = "AppleHttpClientConfiguration:Lifetime";
        public const string PrivateKey = "AppleHttpClientConfiguration:PrivateKey";
    }
}
