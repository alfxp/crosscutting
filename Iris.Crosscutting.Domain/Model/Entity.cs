using System;

namespace Iris.Crosscutting.Domain.Model
{
    public class Entity : IEntity
    {
        public Guid Id { get; set; }
    }
}