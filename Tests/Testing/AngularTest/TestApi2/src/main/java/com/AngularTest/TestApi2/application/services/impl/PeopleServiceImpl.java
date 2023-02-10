package com.AngularTest.TestApi2.application.services.impl;

import lombok.AllArgsConstructor;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import com.AngularTest.TestApi2.application.models.People.PersonCreateDTO;
import com.AngularTest.TestApi2.application.models.People.PersonDTO;
import com.AngularTest.TestApi2.application.models.People.PersonUpdateDTO;
import com.AngularTest.TestApi2.application.services.PeopleService;
import com.AngularTest.TestApi2.data.PersonRepository;
import com.AngularTest.TestApi2.domain.models.Person;
import com.AngularTest.TestApi2.intent.IntentIgnoreBody;
import com.AngularTest.TestApi2.intent.IntentMerge;
import java.time.LocalDateTime;
import java.time.ZoneOffset;
import java.util.Date;
import java.util.List;
import java.util.UUID;
import org.modelmapper.ModelMapper;
import java.time.LocalDate;

@Service
@AllArgsConstructor
@IntentMerge
public class PeopleServiceImpl implements PeopleService {
    private PersonRepository personRepository;
    private ModelMapper mapper;

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public UUID Create(PersonCreateDTO dto) {
        var person = new Person();
        person.setName(dto.getName());
        personRepository.save(person);
        return person.getId();
    }

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public PersonDTO FindById(UUID id) {
        var person = personRepository.findById(id);
        if (!person.isPresent()) {
            return null;
        }
        return PersonDTO.mapFromPerson(person.get(), mapper);
    }

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public List<PersonDTO> FindAll() {
        var people = personRepository.findAll();
        return PersonDTO.mapFromPeople(people, mapper);
    }

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void Update(UUID id, PersonUpdateDTO dto) {
        var person = personRepository.findById(id).get();
        person.setName(dto.getName());
        personRepository.save(person);
    }

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void Delete(UUID id) {
        var person = personRepository.findById(id).get();
        personRepository.delete(person);
    }

}