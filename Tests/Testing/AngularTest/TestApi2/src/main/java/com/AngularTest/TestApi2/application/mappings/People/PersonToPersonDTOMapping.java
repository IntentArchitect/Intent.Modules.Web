package com.AngularTest.TestApi2.application.mappings.People;

import com.AngularTest.TestApi2.domain.models.Person;

import org.modelmapper.PropertyMap;
import com.AngularTest.TestApi2.application.models.People.PersonDTO;

public class PersonToPersonDTOMapping extends PropertyMap<Person, PersonDTO> {
    protected void configure() {
    }
}