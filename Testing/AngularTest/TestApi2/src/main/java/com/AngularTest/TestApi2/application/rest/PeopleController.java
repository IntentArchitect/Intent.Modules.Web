package com.AngularTest.TestApi2.application.rest;

import lombok.AllArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import com.AngularTest.TestApi2.application.models.People.PersonCreateDTO;
import com.AngularTest.TestApi2.application.models.People.PersonDTO;
import com.AngularTest.TestApi2.application.models.People.PersonUpdateDTO;
import com.AngularTest.TestApi2.application.services.PeopleService;
import io.swagger.annotations.Api;
import io.swagger.annotations.ApiOperation;
import java.time.LocalDateTime;
import java.util.List;
import java.util.UUID;
import javax.validation.Valid;
import org.springframework.http.MediaType;
import java.time.LocalDate;

@RestController
@RequestMapping("/api/people")
@Api(value = "PeopleService")
@AllArgsConstructor
public class PeopleController {
    private final PeopleService peopleService;

    @PostMapping
    @ApiOperation(value = "Create")
    public ResponseEntity<UUID> Create(@Valid @RequestBody PersonCreateDTO dto) {
        final UUID result = peopleService.Create(dto);
        if (result == null) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @GetMapping(path = "/{id}")
    @ApiOperation(value = "FindById")
    public ResponseEntity<PersonDTO> FindById(@PathVariable(value = "id") UUID id) {
        final PersonDTO result = peopleService.FindById(id);
        if (result == null) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @GetMapping
    @ApiOperation(value = "FindAll")
    public ResponseEntity<List<PersonDTO>> FindAll() {
        final List<PersonDTO> result = peopleService.FindAll();
        if (result.isEmpty()) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @ResponseStatus(HttpStatus.OK)
    @PutMapping(path = "/{id}")
    @ApiOperation(value = "Update")
    public void Update(@PathVariable(value = "id") UUID id, @Valid @RequestBody PersonUpdateDTO dto) {
        peopleService.Update(id, dto);
    }

    @ResponseStatus(HttpStatus.OK)
    @DeleteMapping(path = "/{id}")
    @ApiOperation(value = "Delete")
    public void Delete(@PathVariable(value = "id") UUID id) {
        peopleService.Delete(id);
    }
}