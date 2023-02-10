using System;
using System.Collections.Generic;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace TestApi.Application.People
{

    public class PersonCreateDTO
    {
        public PersonCreateDTO()
        {
        }

        public static PersonCreateDTO Create(
            string name)
        {
            return new PersonCreateDTO
            {
                Name = name,
            };
        }

        public string Name { get; set; }

    }
}