package com.AngularTest.TestApi2.application.services;

import com.AngularTest.TestApi2.application.models.People.PersonCreateDTO;
import com.AngularTest.TestApi2.application.models.People.PersonDTO;
import com.AngularTest.TestApi2.application.models.People.PersonUpdateDTO;
import java.time.LocalDateTime;
import java.util.List;
import java.util.UUID;
import java.time.LocalDate;


public interface PeopleService {
    UUID Create(PersonCreateDTO dto);

    PersonDTO FindById(UUID id);

    List<PersonDTO> FindAll();

    void Update(UUID id, PersonUpdateDTO dto);

    void Delete(UUID id);

}