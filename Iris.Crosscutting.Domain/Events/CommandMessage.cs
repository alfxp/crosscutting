using MediatR;

namespace Iris.Crosscutting.Domain.Events
{
    public abstract class CommandMessage : IRequest, IRequestBase
    {
        public string MessageType { get; protected set; }

        protected CommandMessage()
        {
            MessageType = GetType().Name;
        }
    }
}