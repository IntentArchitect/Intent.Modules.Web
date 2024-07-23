using System;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;
using NetApplication.Application.Common.Mappings;
using NetApplication.Domain.Entities;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace NetApplication.Application.NetClient
{
    public class ClientDto : IMapFrom<Client>
    {
        public ClientDto()
        {
            Name = null!;
        }

        public string Name { get; set; }
        public Guid Id { get; set; }

        public static ClientDto Create(string name, Guid id)
        {
            return new ClientDto
            {
                Name = name,
                Id = id
            };
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Client, ClientDto>();
        }
    }
}