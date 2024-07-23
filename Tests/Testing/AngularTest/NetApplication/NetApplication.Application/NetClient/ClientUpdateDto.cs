using System;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace NetApplication.Application.NetClient
{
    public class ClientUpdateDto
    {
        public ClientUpdateDto()
        {
            Name = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public static ClientUpdateDto Create(Guid id, string name)
        {
            return new ClientUpdateDto
            {
                Id = id,
                Name = name
            };
        }
    }
}