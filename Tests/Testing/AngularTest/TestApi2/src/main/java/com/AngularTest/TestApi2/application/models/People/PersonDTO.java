package com.AngularTest.TestApi2.application.models.People;

import com.AngularTest.TestApi2.domain.models.Person;

import lombok.Data;
import lombok.NoArgsConstructor;
import java.util.Collection;
import java.util.List;
import java.util.stream.Collectors;
import java.util.UUID;
import lombok.AllArgsConstructor;
import org.modelmapper.ModelMapper;

@AllArgsConstructor
@NoArgsConstructor
@Data
public class PersonDTO {
    private UUID Id;
    private String Name;

    public static PersonDTO mapFromPerson(Person person, ModelMapper mapper) {
        return mapper.map(person, PersonDTO.class);
    }

    public static List<PersonDTO> mapFromPeople(Collection<Person> people, ModelMapper mapper) {
        return people
            .stream()
            .map(person -> PersonDTO.mapFromPerson(person, mapper))
            .collect(Collectors.toList());
    }
}