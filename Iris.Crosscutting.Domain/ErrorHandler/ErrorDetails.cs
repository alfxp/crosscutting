using System;

namespace Iris.Crosscutting.Domain.ErrorHandler
{
    public class ErrorDetails : ErrorResult
    {
        public Exception Details { get; set; }

        public ErrorDetails(ErrorResult errorResult, Exception exception)
        {
            Id = errorResult.Id;
            CreatedAt = errorResult.CreatedAt;
            Errors = errorResult.Errors;
            Url = errorResult.Url;

            Details = exception;
        }
    }
}