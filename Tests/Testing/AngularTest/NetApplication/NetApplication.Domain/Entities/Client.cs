using System;
using System.Collections.Generic;
using Intent.RoslynWeaver.Attributes;
using NetApplication.Domain.Common;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace NetApplication.Domain.Entities
{
    public class Client : IHasDomainEvent
    {
        public Client()
        {
            Name = null!;
        }
        public Guid Id { get; set; }

        public string Name { get; set; }

        public char StatusCode { get; set; }

        public List<DomainEvent> DomainEvents { get; set; } = [];
    }
}