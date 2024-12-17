using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SMARTBusinessTest.Application.Exceptions;

namespace SMARTBusinessTest.Application.Filters
{
    public class ContractExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public ContractExceptionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ContractExceptionFilter>();
        }
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is OperationCanceledException)
            {
                _logger.LogInformation("Request was cancelled, operation took too long.");
                context.Result = new ObjectResult(new
                {
                    message = context.Exception.Message,
                    StatusCode = 400
                });
            }
            else if (context.Exception is InvalidInputDataException)
            {
                context.HttpContext.Response.StatusCode = 409;
                context.Result = new ObjectResult(new
                {
                    message = context.Exception.Message,
                    StatusCode = 409
                });
            }
            else if (context.Exception is OutOfPlacementException)
            {
                context.HttpContext.Response.StatusCode = 409;
                context.Result = new ObjectResult(new
                {
                    message = context.Exception.Message,
                    StatusCode = 409
                });
            }
            else
            {
                _logger.LogInformation(context.Exception.Message);
                context.HttpContext.Response.StatusCode = 500;
                context.Result = new ObjectResult(new
                {
                    message = "The problem on the server side occured.",
                    StatusCode = 500
                });
            }
            context.ExceptionHandled = true;
        }
    }
}
