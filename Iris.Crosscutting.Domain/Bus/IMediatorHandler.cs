using Iris.Crosscutting.Domain.Commands;
using Iris.Crosscutting.Domain.Events;
using Iris.Crosscutting.Domain.Notifications;
using Iris.Crosscutting.Domain.Queries;
using MediatR;
using System.Threading.Tasks;

namespace Iris.Crosscutting.Domain.Bus
{
    public interface IMediatorHandler
    {
        Task SendCommand<T>(T command) where T : Command;
        Task<TResponse> SendQuery<TResponse>(Query<TResponse> query) where TResponse : class;
        Task RaiseEvent<T>(T @event) where T : Event;
        bool HasNotification();
        INotificationHandler<DomainNotification> GetNotificationHandler();
    }
}