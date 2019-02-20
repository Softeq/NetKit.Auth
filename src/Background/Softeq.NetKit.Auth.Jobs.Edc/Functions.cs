// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using EnsureThat;
using Softeq.NetKit.Auth.Common.EmailTemplates;
using Softeq.NetKit.Auth.Common.Logger;
using Softeq.NetKit.Auth.Integration.Edc;
using Softeq.NetKit.Auth.Integration.Edc.Services;
using Softeq.NetKit.Auth.Integration.Email;
using Softeq.NetKit.Auth.Integration.Email.Dto;
using Softeq.NetKit.Auth.Jobs.Edc.Configurations;
using Softeq.NetKit.Auth.Jobs.Edc.Notifications;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Serilog;
using Softeq.NetKit.Integrations.EventLog;
using Softeq.NetKit.Integrations.EventLog.Abstract;
using Softeq.Serilog.Extension;

namespace Softeq.NetKit.Auth.Jobs.Edc
{
    public class Functions
    {
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly EdcConfiguration _edcConfiguration;
        private readonly IEdcPublishService _edcPublishService;
        private readonly JobsConfiguration _jobsConfiguration;
        private readonly IEmailService _emailService;
        private readonly ILogger _logger;
        private readonly ILoggerHelper _loggerHelper;
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        
        public Functions(IIntegrationEventLogService eventLogService,
            EdcConfiguration edcConfiguration,
            IEdcPublishService edcPublishService,
            JobsConfiguration jobsConfiguration,
            IEmailService emailService,
            ILogger logger,
            ILoggerHelper loggerHelper,
            IEmailTemplateProvider emailTemplateProvider)
        {
            Ensure.That(eventLogService).IsNotNull();
            Ensure.That(edcConfiguration).IsNotNull();
            Ensure.That(edcPublishService).IsNotNull();
            Ensure.That(jobsConfiguration).IsNotNull();
            Ensure.That(emailService).IsNotNull();
            Ensure.That(logger).IsNotNull();
            Ensure.That(loggerHelper).IsNotNull();
            Ensure.That(emailTemplateProvider).IsNotNull();

            _eventLogService = eventLogService;
            _edcConfiguration = edcConfiguration;
            _edcPublishService = edcPublishService;
            _jobsConfiguration = jobsConfiguration;
            _emailService = emailService;
            _logger = logger;
            _loggerHelper = loggerHelper;
            _emailTemplateProvider = emailTemplateProvider;
        }

        [FunctionName("HandleNotCompletedEdcEvents")]
        public async Task RunNotCompletedEdcEventsHandlerAsync([TimerTrigger("%Jobs:HandleNotCompletedEdcEvents:Schedule%")] TimerInfo timer)
        {
            await FunctionTemplate("HandleNotCompletedEdcEvents", async () =>
            {
                var tillDate = DateTime.UtcNow.AddMinutes(-_edcConfiguration.MessageTimeToLiveInMinutes);

                var notCompletedEventLogs = await _eventLogService.GetAsync(log =>
                    log.StateId != (int) EventStateEnum.Completed
                    && log.CreationTime < tillDate);

                foreach (var eventLog in notCompletedEventLogs)
                {
                    _logger.Event($"{_jobsConfiguration.LogEventId}.HandleNotCompletedEdcEvents").With
                        .Message($"Event Failed: EventId:{eventLog.EventId}, EventName: {eventLog.EventTypeName}, EventContent: { JsonConvert.SerializeObject(eventLog.Content)}")
                        .AsError();

                    if (eventLog.TimesSent >= _jobsConfiguration.HandleNotCompletedEdcEvents.MaxAttemptsCountBeforeEmailSending)
                    {
                        var baseEmailTemplate = _emailTemplateProvider.GetBaseTemplateHtml();

                        var serviceBusErrorEmailTemplate = _emailTemplateProvider.GetTemplateHtml(
                            _jobsConfiguration.HandleNotCompletedEdcEvents.ServiceBusErrorEmailTemplateName);

                        await _emailService.SendNotificationAsync(new ServiceBusErrorEmailNotification(
                            new ServiceBusErrorEmailNotificationTemplateModel(eventLog.EventId),
                            baseEmailTemplate, serviceBusErrorEmailTemplate,
                            new RecipientDto(_jobsConfiguration.HandleNotCompletedEdcEvents.ServiceBusErrorEmailRecipients, null)));
                    }

                    eventLog.StateId = (int) EventStateEnum.PublishedFailed;
                    await _eventLogService.UpdateAsync(eventLog);
                    await _edcPublishService.RepublishAsync(eventLog.EventId);
                }
            });
        }

        private async Task FunctionTemplate(string functionName, Func<Task> process)
        {
            _loggerHelper.ResetCorrelationContextAndCorrelationId(Guid.NewGuid().ToString());

            _logger.Event(_jobsConfiguration.LogEventId).With.Message($"\"{functionName}\" job started at: {DateTimeOffset.UtcNow}").AsInformation();
            try
            {
                await process();
            }
            catch (Exception exception)
            {
                _logger.Event(_jobsConfiguration.LogEventId).With.Exception(exception).Message($"Exception occured while running \"{functionName}\" job.").AsError();
            }
            finally
            {
                _logger.Event(_jobsConfiguration.LogEventId).With.Message($"\"{functionName}\" job completed at: {DateTimeOffset.UtcNow}").AsInformation();
            }
        }
    }
}