using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;
using TestApi.Application.Interfaces;
using TestApi.Application.People;
using TestApi.Domain.Entities;
using TestApi.Domain.Repositories;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.ServiceImplementations.ServiceImplementation", Version = "1.0")]

namespace TestApi.Application.Implementation
{
    [IntentManaged(Mode.Merge)]
    public class PeopleService : IPeopleService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public PeopleService(IPersonRepository personRepository, IMapper mapper)
        {
            _personRepository = personRepository;
            _mapper = mapper;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<Guid> Create(PersonCreateDTO dto)
        {
            var newPerson = new Person
            {
                Name = dto.Name,
            };

            _personRepository.Add(newPerson);
            await _personRepository.UnitOfWork.SaveChangesAsync();
            return newPerson.Id;
        }



        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<PersonDTO> FindById(Guid id)
        {
            var element = await _personRepository.FindByIdAsync(id);
            return element.MapToPersonDTO(_mapper);
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<List<PersonDTO>> FindAll()
        {
            var elements = await _personRepository.FindAllAsync();
            return elements.MapToPersonDTOList(_mapper);
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task Update(Guid id, PersonUpdateDTO dto)
        {
            var existingPerson = await _personRepository.FindByIdAsync(id);
            existingPerson.Name = dto.Name;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task Delete(Guid id)
        {
            var existingPerson = await _personRepository.FindByIdAsync(id);
            _personRepository.Remove(existingPerson);
        }

        public void Dispose()
        {
        }
    }
}