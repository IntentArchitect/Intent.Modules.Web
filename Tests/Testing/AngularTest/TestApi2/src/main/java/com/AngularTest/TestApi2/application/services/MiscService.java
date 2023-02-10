package com.AngularTest.TestApi2.application.services;

import com.AngularTest.TestApi2.application.models.Misc.DateDTO;
import com.AngularTest.TestApi2.application.models.People.PersonDTO;
import com.AngularTest.TestApi2.application.models.People.PersonUpdateDTO;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.UUID;


public interface MiscService {
    PersonDTO GetWithQueryParam(UUID idParam);

    PersonDTO GetWithRouteParam(UUID routeId);

    void PostWithHeaderParam(String param);

    void PostWithBodyParam(PersonUpdateDTO param);

    int GetWithPrimitiveResultInt();

    int GetWithPrimitiveResultWrapInt();

    boolean GetWithPrimitiveResultBool();

    boolean GetWithPrimitiveResultWrapBool();

    String GetWithPrimitiveResultStr();

    String GetWithPrimitiveResultWrapStr();

    LocalDateTime GetWithPrimitiveResultDate();

    LocalDateTime GetWithPrimitiveResultWrapDate();

    void PostDateParam(LocalDate date, LocalDateTime datetime);

    void PostDateParamDto(DateDTO dto);

}