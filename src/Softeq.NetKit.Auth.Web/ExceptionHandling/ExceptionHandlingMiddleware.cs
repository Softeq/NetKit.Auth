// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.NetKit.Auth.Common.Exceptions;
using Softeq.NetKit.Auth.Common.Exceptions.Exceptions;
using Softeq.NetKit.Auth.Common.Exceptions.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using Softeq.Serilog.Extension;

namespace Softeq.NetKit.Auth.Web.ExceptionHandling
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IHostingEnvironment env)
        {
            try
            {
                await _next(context);
            }
            catch (NetKitAuthValidationException ex)
            {
                await HandleExceptionAsync(context, env, ex, StatusCodes.Status400BadRequest, ex.ErrorCode);
            }
            catch (NetKitAuthUnauthorizedException ex)
            {
                await HandleExceptionAsync(context, env, ex, StatusCodes.Status401Unauthorized, ex.ErrorCode);
            }
            catch (NetKitAuthForbiddenException ex)
            {
                await HandleExceptionAsync(context, env, ex, StatusCodes.Status403Forbidden, ex.ErrorCode);
            }
            catch (NetKitAuthNotFoundException ex)
            {
                await HandleExceptionAsync(context, env, ex, StatusCodes.Status404NotFound, ex.ErrorCode);
            }
            catch (NetKitAuthConflictException ex)
            {
                await HandleExceptionAsync(context, env, ex, StatusCodes.Status409Conflict, ex.ErrorCode);
            }
            catch (NetKitAuthException ex)
            {
                await HandleExceptionAsync(context, env, ex, StatusCodes.Status400BadRequest, ex.ErrorCode);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, env, ex, StatusCodes.Status500InternalServerError, ErrorCode.UnknownError);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, IHostingEnvironment env, Exception ex, int statusCode, ErrorCode errorCode)
        {
            _logger.Event("UnhandledExceptionCaughtByMiddleware")
                .With.Exception(ex)
                .Message("Exception was caught by exception handling middleware. Status code = {StatusCode}; error code = {ErrorCode}", statusCode, errorCode)
                .AsError();

            if (context.Response.HasStarted)
            {
                _logger.Event("UnableToModifyResponse")
                    .With.Exception(ex)
                    .Message("The response has already started, the exception handling middleware will not be executed.")
                    .AsError();
                throw ex;
            }

            var errorResponseModel = ex is NetKitAuthValidationException validationException ?
                new ValidationErrorResponseModel(errorCode, ex.Message, validationException.Errors) :
                new ErrorResponseModel(errorCode, ex.Message);

            if (!env.IsProduction())
            {
                errorResponseModel.StackTrace = ex.StackTrace;
            }
            var response = JsonConvert.SerializeObject(errorResponseModel);

            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(response);
        }
    }
}