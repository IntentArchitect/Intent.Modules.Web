using System;
using System.Collections.Generic;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;
using TestApi.Application.Common.Mappings;
using TestApi.Domain.Entities;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace TestApi.Application.People
{

    public class PersonDTO : IMapFrom<Person>
    {
        public PersonDTO()
        {
        }

        public static PersonDTO Create(
            Guid id,
            string name)
        {
            return new PersonDTO
            {
                Id = id,
                Name = name,
            };
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Person, PersonDTO>();
        }
    }
}