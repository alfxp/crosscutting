using Iris.Crosscutting.Domain.Bus;
using Iris.Crosscutting.Domain.Notifications;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iris.Crosscutting.Domain.Commands
{
    public abstract class MediatorCommandHandler<TCommand> : AsyncRequestHandler<TCommand>
          where TCommand : Command
    {
        protected IMediatorHandler _mediator { get; }

        protected MediatorCommandHandler(IMediatorHandler mediator)
        {
            _mediator = mediator;
        }

        public abstract Task AfterValidation(TCommand request);

        protected override Task Handle(TCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);

                return Task.CompletedTask;
            }

            return AfterValidation(request);
        }

        protected void NotifyValidationErrors(TCommand message)
        {
            foreach (var error in message.ValidationResult.Errors)
            {
                _mediator.RaiseEvent(new DomainNotification(message.MessageType, error.ErrorMessage));
            }
        }

        protected void NotifyError(string code, string message)
        {
            _mediator.RaiseEvent(new DomainNotification(code, message));
        }

        protected void NotifyError(string message) => NotifyError(string.Empty, message);

        protected Task NotifyWithError(string message)
        {
            NotifyError(message);

            return Task.CompletedTask;
        }

        protected void NotifyError(IEnumerable<string> messages) { foreach (var message in messages) NotifyError(message); }

        protected bool HasNotification() => _mediator.HasNotification();

        protected IEnumerable<string> Errors => ((DomainNotificationHandler)_mediator.GetNotificationHandler())
            .GetNotifications()
            .Select(t => t.Value);
    }
}