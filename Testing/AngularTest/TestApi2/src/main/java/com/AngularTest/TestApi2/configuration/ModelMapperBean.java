package com.AngularTest.TestApi2.configuration;

import org.modelmapper.ModelMapper;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import com.AngularTest.TestApi2.application.mappings.People.PersonToPersonDTOMapping;

@Configuration
public class ModelMapperBean {
    @Bean
    public ModelMapper modelMapper() {
        var modelMapper = new ModelMapper();

        InitializeMappings(modelMapper);

        return modelMapper;
    }

    private void InitializeMappings(ModelMapper modelMapper) {
        modelMapper.addMappings(new PersonToPersonDTOMapping());
    }
}
