using FluentValidation.Results;
using Iris.Crosscutting.Common.Extensions;
using Iris.Crosscutting.Domain.Events;
using System;

namespace Iris.Crosscutting.Domain.Queries
{
    public abstract class Query<TResponse> : QueryMessage<TResponse>
    {
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; set; } = new ValidationResult();

        protected Query()
        {
            Timestamp = DateTime.Now.ToBrazilianTimezone();
        }

        public abstract bool IsValid();
    }
}