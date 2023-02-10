package com.AngularTest.TestApi2.application.rest;

import lombok.AllArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import com.AngularTest.TestApi2.application.models.Misc.DateDTO;
import com.AngularTest.TestApi2.application.models.People.PersonDTO;
import com.AngularTest.TestApi2.application.models.People.PersonUpdateDTO;
import com.AngularTest.TestApi2.application.services.MiscService;
import io.swagger.annotations.Api;
import io.swagger.annotations.ApiOperation;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.UUID;
import javax.validation.Valid;

@RestController
@RequestMapping("/api/misc")
@Api(value = "MiscService")
@AllArgsConstructor
public class MiscController {
    private final MiscService miscService;

    @GetMapping(path = "/getwithqueryparam")
    @ApiOperation(value = "GetWithQueryParam")
    public ResponseEntity<PersonDTO> GetWithQueryParam(@RequestParam(value = "idParam") UUID idParam) {
        final PersonDTO result = miscService.GetWithQueryParam(idParam);
        if (result == null) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @GetMapping(path = "/getwithrouteparam/{routeId}")
    @ApiOperation(value = "GetWithRouteParam")
    public ResponseEntity<PersonDTO> GetWithRouteParam(@PathVariable(value = "routeId") UUID routeId) {
        final PersonDTO result = miscService.GetWithRouteParam(routeId);
        if (result == null) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @ResponseStatus(HttpStatus.OK)
    @PostMapping(path = "/postwithheaderparam")
    @ApiOperation(value = "PostWithHeaderParam")
    public void PostWithHeaderParam(@RequestHeader("MY_HEADER") String param) {
        miscService.PostWithHeaderParam(param);
    }

    @ResponseStatus(HttpStatus.OK)
    @PostMapping(path = "/postwithbodyparam")
    @ApiOperation(value = "PostWithBodyParam")
    public void PostWithBodyParam(@Valid  PersonUpdateDTO param) {
        miscService.PostWithBodyParam(param);
    }

    @GetMapping(path = "/getwithprimitiveresultint")
    @ApiOperation(value = "GetWithPrimitiveResultInt")
    public ResponseEntity<Integer> GetWithPrimitiveResultInt() {
        final int result = miscService.GetWithPrimitiveResultInt();
        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @GetMapping(path = "/getwithprimitiveresultwrapint")
    @ApiOperation(value = "GetWithPrimitiveResultWrapInt")
    public ResponseEntity<JsonResponse<Integer>> GetWithPrimitiveResultWrapInt() {
        final int result = miscService.GetWithPrimitiveResultWrapInt();
        return new ResponseEntity<>(new JsonResponse<Integer>(result), HttpStatus.OK);
    }

    @GetMapping(path = "/getwithprimitiveresultbool")
    @ApiOperation(value = "GetWithPrimitiveResultBool")
    public ResponseEntity<Boolean> GetWithPrimitiveResultBool() {
        final boolean result = miscService.GetWithPrimitiveResultBool();
        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @GetMapping(path = "/getwithprimitiveresultwrapbool")
    @ApiOperation(value = "GetWithPrimitiveResultWrapBool")
    public ResponseEntity<JsonResponse<Boolean>> GetWithPrimitiveResultWrapBool() {
        final boolean result = miscService.GetWithPrimitiveResultWrapBool();
        return new ResponseEntity<>(new JsonResponse<Boolean>(result), HttpStatus.OK);
    }

    @GetMapping(path = "/getwithprimitiveresultstr")
    @ApiOperation(value = "GetWithPrimitiveResultStr")
    public ResponseEntity<String> GetWithPrimitiveResultStr() {
        final String result = miscService.GetWithPrimitiveResultStr();
        if (result == null) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @GetMapping(path = "/getwithprimitiveresultwrapstr")
    @ApiOperation(value = "GetWithPrimitiveResultWrapStr")
    public ResponseEntity<JsonResponse<String>> GetWithPrimitiveResultWrapStr() {
        final String result = miscService.GetWithPrimitiveResultWrapStr();
        if (result == null) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
        return new ResponseEntity<>(new JsonResponse<String>(result), HttpStatus.OK);
    }

    @GetMapping(path = "/getwithprimitiveresultdate")
    @ApiOperation(value = "GetWithPrimitiveResultDate")
    public ResponseEntity<LocalDateTime> GetWithPrimitiveResultDate() {
        final LocalDateTime result = miscService.GetWithPrimitiveResultDate();
        if (result == null) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @GetMapping(path = "/getwithprimitiveresultwrapdate")
    @ApiOperation(value = "GetWithPrimitiveResultWrapDate")
    public ResponseEntity<LocalDateTime> GetWithPrimitiveResultWrapDate() {
        final LocalDateTime result = miscService.GetWithPrimitiveResultWrapDate();
        if (result == null) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @ResponseStatus(HttpStatus.OK)
    @PostMapping(path = "/postdateparam")
    @ApiOperation(value = "PostDateParam")
    public void PostDateParam(@RequestParam(value = "date") LocalDate date, @RequestParam(value = "datetime") LocalDateTime datetime) {
        miscService.PostDateParam(date, datetime);
    }

    @ResponseStatus(HttpStatus.OK)
    @PostMapping(path = "/postdateparamdto")
    @ApiOperation(value = "PostDateParamDto")
    public void PostDateParamDto(@Valid @RequestBody DateDTO dto) {
        miscService.PostDateParamDto(dto);
    }
}
