using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;
using TestApi.Domain.Entities;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.MappingExtensions", Version = "1.0")]

namespace TestApi.Application.People
{
    public static class PersonDTOMappingExtensions
    {
        public static PersonDTO MapToPersonDTO(this IPerson projectFrom, IMapper mapper)
        {
            return mapper.Map<PersonDTO>(projectFrom);
        }

        public static List<PersonDTO> MapToPersonDTOList(this IEnumerable<IPerson> projectFrom, IMapper mapper)
        {
            return projectFrom.Select(x => x.MapToPersonDTO(mapper)).ToList();
        }
    }
}