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
        public char StatusCode { get; set; }

        public static ClientUpdateDto Create(Guid id, string name, char statusCode)
        {
            return new ClientUpdateDto
            {
                Id = id,
                Name = name,
                StatusCode = statusCode
            };
        }
    }
}