using System;

namespace Iris.Crosscutting.Domain.Model
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}