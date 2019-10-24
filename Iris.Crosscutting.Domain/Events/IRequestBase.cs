namespace Iris.Crosscutting.Domain.Events
{
    public interface IRequestBase
    {
        string MessageType { get; }
    }
}