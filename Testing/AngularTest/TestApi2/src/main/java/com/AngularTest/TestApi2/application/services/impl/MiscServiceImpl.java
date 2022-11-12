package com.AngularTest.TestApi2.application.services.impl;

import lombok.AllArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import com.AngularTest.TestApi2.application.models.Misc.DateDTO;
import com.AngularTest.TestApi2.application.models.People.PersonDTO;
import com.AngularTest.TestApi2.application.models.People.PersonUpdateDTO;
import com.AngularTest.TestApi2.application.services.MiscService;
import com.AngularTest.TestApi2.intent.IntentIgnoreBody;
import com.AngularTest.TestApi2.intent.IntentMerge;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.ZoneOffset;
import java.util.UUID;

@Service
@AllArgsConstructor
@IntentMerge
public class MiscServiceImpl implements MiscService {

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public PersonDTO GetWithQueryParam(UUID idParam) {
        return new PersonDTO(UUID.randomUUID(), "Test " + UUID.randomUUID());
    }

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public PersonDTO GetWithRouteParam(UUID routeId) {
        return new PersonDTO(UUID.randomUUID(), "Test " + UUID.randomUUID());
    }

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void PostWithHeaderParam(String param) {

    }

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void PostWithBodyParam(PersonUpdateDTO param) {

    }

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public int GetWithPrimitiveResultInt() {
        return 12;
    }

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public int GetWithPrimitiveResultWrapInt() {
        return 13;
    }

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public boolean GetWithPrimitiveResultBool() {
        return true;
    }

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public boolean GetWithPrimitiveResultWrapBool() {
        return false;
    }

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public String GetWithPrimitiveResultStr() {
        return "Primitive string value";
    }

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public String GetWithPrimitiveResultWrapStr() {
        return "Wrapped Primitive string value";
    }

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public LocalDateTime GetWithPrimitiveResultDate() {
        return LocalDateTime.now( ZoneOffset.UTC );
    }

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public LocalDateTime GetWithPrimitiveResultWrapDate() {
        return LocalDateTime.now();
    }

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void PostDateParam(LocalDate date, LocalDateTime datetime) {

    }

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void PostDateParamDto(DateDTO dto) {

    }
}