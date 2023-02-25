using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using CompanyName.Model.Exceptions;

namespace CompanyName.Api.Filters
{
    /// <summary>
    /// The global exception handler for the application.
    /// </summary>
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<GlobalExceptionFilter> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalExceptionFilter"/> class.
        /// </summary>
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc/>
        public override void OnException(ExceptionContext context)
        {
            if (context != null && context?.Exception != null)
            {
                string traceId = Activity.Current?.Id ?? context?.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString();

                switch (context.Exception)
                {
                    case ApiException exception:

                        if (exception.HasErrors)
                        {
                            foreach (var item in exception.Errors)
                            {
                                context.ModelState.AddModelError(item.Key, item.Value);
                            }
                        }

                        ValidationProblemDetails errorResponse = new(context.ModelState)
                        {
                            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                            Status = (int)HttpStatusCode.BadRequest,
                            Title = "One or more validation errors occurred.",
                            Detail = exception.Message ?? "One or more validation errors occurred."
                        };

                        errorResponse.Extensions["traceId"] = traceId;

                        context.Result = new BadRequestObjectResult(errorResponse);
                        context.ExceptionHandled = true;
                        break;


                    case NotFoundException exception:

                        ProblemDetails notFoundResponse = new()
                        {
                            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                            Status = (int)HttpStatusCode.NotFound,
                            Title = "Entity not found.",
                            Detail = exception.Message ?? "Entity not found.",
                        };

                        notFoundResponse.Extensions["traceId"] = traceId;

                        context.Result = new NotFoundObjectResult(notFoundResponse);
                        context.ExceptionHandled = true;
                        break;

                    default:
                        var message = $"An unhandled exception occurred, please contact administrator with ReferenceId:{traceId}";

                        ProblemDetails internalServerError = new()
                        {
                            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                            Status = (int)HttpStatusCode.InternalServerError,
                            Title = "Internal Server Error",
                            Detail = message
                        };

                        internalServerError.Extensions["traceId"] = traceId;

                        var exceptionResult = new ObjectResult(internalServerError)
                        {
                            StatusCode = (int)HttpStatusCode.InternalServerError,
                        };
                        context.Result = exceptionResult;

                        //log exception to any logging framework
                        logger.LogCritical(message: message);
                        logger.LogError(context.Exception, context.Exception.Message);
                        break;
                }

            }

            base.OnException(context);
        }
    }
}
