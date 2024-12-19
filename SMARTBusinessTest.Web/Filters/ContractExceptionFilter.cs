using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SMARTBusinessTest.Application.Constants;
using SMARTBusinessTest.Domain.Exceptions;

namespace SMARTBusinessTest.Web.Filters
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
                _logger.LogInformation(ExceptionConstants.TokenCancelled);
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new ObjectResult(new ContractExceptionResult
                {
                    Message = context.Exception.Message,
                    StatusCode = 400
                });
            }
            else if (context.Exception is InvalidInputDataException)
            {
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new ObjectResult(new ContractExceptionResult
                {
                    Message = context.Exception.Message,
                    StatusCode = 400
                });
            }
            else if (context.Exception is OutOfPlacementException)
            {
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new ObjectResult(new ContractExceptionResult
                {
                    Message = context.Exception.Message,
                    StatusCode = 400
                });
            }
            else
            {
                _logger.LogInformation(context.Exception.Message);
                context.HttpContext.Response.StatusCode = 500;
                context.Result = new ObjectResult(new ContractExceptionResult
                {
                    Message = ExceptionConstants.InternalServerError,
                    StatusCode = 500
                });
            }
            context.ExceptionHandled = true;
        }
    }
}
