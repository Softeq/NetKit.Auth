// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.IO;
using Autofac;
using CorrelationId;
using EnsureThat;
using Softeq.NetKit.Auth.AppServices.Utility;
using Softeq.NetKit.Auth.Common.EmailTemplates;
using Softeq.NetKit.Auth.Common.Utility.Hashing;
using Softeq.NetKit.Auth.Common.Utility.TokenProvider;
using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.Integration.Edc;
using Softeq.NetKit.Auth.Integration.Edc.Handlers;
using Softeq.NetKit.Auth.Integration.Email;
using Softeq.NetKit.Auth.Web.Middleware;
using Softeq.NetKit.Auth.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Softeq.NetKit.Auth.Web.DI
{
	public class StartupModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			#region IntegrationServices
            
		    builder
		        .Register(context =>
		        {
		            var configuration = context.Resolve<IConfiguration>();

		            var connectionString = configuration["AzureServiceBus:ConnectionString"];
		            Ensure.That(connectionString).IsNotNullOrWhiteSpace();

		            var topicClientName = configuration["AzureServiceBus:TopicName"];
		            Ensure.That(topicClientName).IsNotNullOrWhiteSpace();

		            var subscriptionClientName = configuration["AzureServiceBus:SubscriptionName"];
		            Ensure.That(subscriptionClientName).IsNotNullOrWhiteSpace();

		            var callAutoCancelQueue = configuration["AzureServiceBus:QueueName"];
		            Ensure.That(callAutoCancelQueue).IsNotNullOrWhiteSpace();

		            var messageTimeToLiveInMinutes = configuration["AzureServiceBus:MessageTimeToLiveInMinutes"];
		            Ensure.That(messageTimeToLiveInMinutes).IsNotNullOrWhiteSpace();

		            var eventPublisherId = configuration["AzureServiceBus:EventPublisherId"];
		            Ensure.That(eventPublisherId).IsNotNullOrWhiteSpace();

		            return new EdcConfiguration(connectionString, topicClientName, subscriptionClientName,
		                callAutoCancelQueue, Convert.ToInt32(messageTimeToLiveInMinutes), eventPublisherId);
		        })
		        .As<EdcConfiguration>()
		        .SingleInstance();

            builder.Register(x =>
			{
				var dataProtectorTokenProvider = x.Resolve<IDataProtectionProvider>();
				var options = x.Resolve<IOptions<DefaultDataProtectorTokenProviderOptions>>();
				return new DefaultDataProtectorTokenProvider<User>(dataProtectorTokenProvider, options);
			});

			builder.Register(x =>
			{
				var options = x.Resolve<IOptions<DefaultPasswordHasherOptions>>();
				return new DefaultPasswordHasher<User>(options);
			});

			builder.Register(context =>
				{
					var appConfig = context.Resolve<IConfiguration>();

					return new AuthApiUrlConfiguration(
						appConfig[ConfigurationSettings.ApplicationUrl],
						appConfig[ConfigurationSettings.AuthResetPasswordPath],
						appConfig[ConfigurationSettings.ConfirmEmailPath]);
				})
				.As<AuthApiUrlConfiguration>();

			builder.Register(context =>
				{
					var appConfig = context.Resolve<IConfiguration>();

					return new PasswordConfiguration
					{
						UniqueCount = Convert.ToInt32(appConfig[ConfigurationSettings.PasswordUniqueCount]),
						ActivePeriodInDays = Convert.ToInt32(appConfig[ConfigurationSettings.PasswordActivePeriodInDays]),
						TokenLifeTimeInMinutes = Convert.ToInt32(appConfig[ConfigurationSettings.DataProtectorProviderTokenLifespan])
					};
				})
				.As<PasswordConfiguration>();

		    builder.Register(context =>
		        {
		            var appConfig = context.Resolve<IConfiguration>();

		            return new UserPasswordConfiguration
		            {
                        RequiredLength = Convert.ToInt32(appConfig[ConfigurationSettings.UserPasswordRequiredLength]),
                        MaximumLength = Convert.ToInt32(appConfig[ConfigurationSettings.UserPasswordMaximumLength]),
                        Regex = appConfig[ConfigurationSettings.UserPasswordRegex]
                    };
		        })
		        .As<UserPasswordConfiguration>();

		    builder.Register(context =>
		        {
		            var appConfig = context.Resolve<IConfiguration>();

		            return new UserEmailConfiguration
		            {
		                MaximumLength = Convert.ToInt32(appConfig[ConfigurationSettings.UserEmailMaximumLength])
		            };
		        })
		        .As<UserEmailConfiguration>();
            
            builder.Register(context =>
		        {
		            var configuration = context.Resolve<IConfiguration>();

		            return new EmailConfiguration(
		                configuration[ConfigurationSettings.SendGridApiKey],
		                configuration[ConfigurationSettings.SendGridSenderEmail],
		                configuration[ConfigurationSettings.SendGridSenderEmailName]);
		        })
		        .As<EmailConfiguration>()
		        .SingleInstance();

		    builder.Register(context =>
		        {
		            var templatesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
                    
                    return new EmailTemplateConfiguration(templatesFolderPath);
		        })
		        .As<EmailTemplateConfiguration>()
		        .SingleInstance();

            builder.Register(context =>
		        {
		            var configuration = context.Resolve<IConfiguration>();

		            return new EmailTemplatesConfiguration(
		                configuration[ConfigurationSettings.EmailConfirmationEmailTemplateName],
		                configuration[ConfigurationSettings.ResetPasswordEmailTemplateName],
		                configuration[ConfigurationSettings.ChangePasswordEmailTemplateName],
		                configuration[ConfigurationSettings.PasswordHasExpiredEmailTemplateName]);
		        })
		        .As<EmailTemplatesConfiguration>()
		        .SingleInstance();

            #endregion

            #region Handlers

            builder.RegisterType<ApiKeyAuthorizationHandler>().As<IAuthorizationHandler>();
			builder.RegisterType<UserStatusAuthorizationHandler>().As<IAuthorizationHandler>();
			builder.RegisterType<PendingAllowedAuthorizationRequirementHandler>().As<IAuthorizationHandler>();
			builder.RegisterType<CompletedEventHandler>();

			#endregion

			builder.RegisterType<CorrelationContextAccessor>().As<ICorrelationContextAccessor>().SingleInstance();
			builder.RegisterType<CorrelationContextFactory>().As<ICorrelationContextFactory>().InstancePerDependency();
		}
	}
}
