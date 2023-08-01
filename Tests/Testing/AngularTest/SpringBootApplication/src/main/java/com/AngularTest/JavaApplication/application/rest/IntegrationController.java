package com.AngularTest.JavaApplication.application.rest;

import lombok.AllArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import com.AngularTest.JavaApplication.application.models.Integration.CustomDTO;
import com.AngularTest.JavaApplication.application.services.IntegrationService;
import com.AngularTest.JavaApplication.exceptions.Integration.TestException;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.tags.Tag;
import java.util.UUID;
import javax.validation.Valid;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.MediaType;
import org.springframework.web.server.ResponseStatusException;
import org.springframework.http.HttpStatus;

@RestController
@Slf4j
@RequestMapping("/api/integration")
@Tag(name = "IntegrationService")
@AllArgsConstructor
public class IntegrationController {
    private final IntegrationService integrationService;

    @GetMapping(path = "/query-param-op")
    @Operation(summary = "QueryParamOp")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "200", description = "Returns the specified CustomDTO."),
        @ApiResponse(responseCode = "400", description = "One or more validation errors have occurred."),
        @ApiResponse(responseCode = "404", description = "Can\'t find an CustomDTO with the parameters provided.") })
    public ResponseEntity<CustomDTO> QueryParamOp(@Parameter(required = true) @RequestParam(value = "param1") String param1, @Parameter(required = true) @RequestParam(value = "param2") int param2) {
        final CustomDTO result = integrationService.QueryParamOp(param1, param2);
        if (result == null) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }

        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @ResponseStatus(HttpStatus.CREATED)
    @PostMapping(path = "/header-param-op")
    @Operation(summary = "HeaderParamOp")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "201", description = "Successfully created."),
        @ApiResponse(responseCode = "400", description = "One or more validation errors have occurred.") })
    public void HeaderParamOp(@Parameter(required = true) @RequestHeader("MY-HEADER") String param1) {
        integrationService.HeaderParamOp(param1);
    }

    @ResponseStatus(HttpStatus.CREATED)
    @PostMapping(path = "/route-param-op/{param1}")
    @Operation(summary = "RouteParamOp")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "201", description = "Successfully created."),
        @ApiResponse(responseCode = "400", description = "One or more validation errors have occurred.") })
    public void RouteParamOp(@Parameter(required = true) @PathVariable(value = "param1") String param1) {
        integrationService.RouteParamOp(param1);
    }

    @ResponseStatus(HttpStatus.CREATED)
    @PostMapping(path = "/body-param-op")
    @Operation(summary = "BodyParamOp")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "201", description = "Successfully created."),
        @ApiResponse(responseCode = "400", description = "One or more validation errors have occurred.") })
    public void BodyParamOp(@Valid @Parameter(required = true) @RequestBody CustomDTO param1) {
        integrationService.BodyParamOp(param1);
    }

    @ResponseStatus(HttpStatus.CREATED)
    @PostMapping(path = "/throws-exception")
    @Operation(summary = "ThrowsException")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "201", description = "Successfully created.") })
    public void ThrowsException() {
        try {
            integrationService.ThrowsException();
        } catch (TestException e) {
            log.error(e.getMessage(), e);
            throw new ResponseStatusException(HttpStatus.INTERNAL_SERVER_ERROR);
        }
    }

    @GetMapping(path = "/wrapped-primitive-guid")
    @Operation(summary = "GetWrappedPrimitiveGuid")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "200", description = "Returns the specified UUID."),
        @ApiResponse(responseCode = "404", description = "Can\'t find an UUID with the parameters provided.") })
    public ResponseEntity<JsonResponse<UUID>> GetWrappedPrimitiveGuid() {
        final UUID result = integrationService.GetWrappedPrimitiveGuid();

        return new ResponseEntity<>(new JsonResponse<UUID>(result), HttpStatus.OK);
    }

    @GetMapping(path = "/wrapped-primitive-string")
    @Operation(summary = "GetWrappedPrimitiveString")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "200", description = "Returns the specified String."),
        @ApiResponse(responseCode = "404", description = "Can\'t find an String with the parameters provided.") })
    public ResponseEntity<JsonResponse<String>> GetWrappedPrimitiveString() {
        final String result = integrationService.GetWrappedPrimitiveString();

        return new ResponseEntity<>(new JsonResponse<String>(result), HttpStatus.OK);
    }

    @GetMapping(path = "/wrapped-primitive-int")
    @Operation(summary = "GetWrappedPrimitiveInt")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "200", description = "Returns the specified int."),
        @ApiResponse(responseCode = "404", description = "Can\'t find an int with the parameters provided.") })
    public ResponseEntity<JsonResponse<Integer>> GetWrappedPrimitiveInt() {
        final int result = integrationService.GetWrappedPrimitiveInt();

        return new ResponseEntity<>(new JsonResponse<Integer>(result), HttpStatus.OK);
    }

    @GetMapping(path = "/primitive-guid")
    @Operation(summary = "GetPrimitiveGuid")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "200", description = "Returns the specified UUID."),
        @ApiResponse(responseCode = "404", description = "Can\'t find an UUID with the parameters provided.") })
    public ResponseEntity<UUID> GetPrimitiveGuid() {
        final UUID result = integrationService.GetPrimitiveGuid();

        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @GetMapping(path = "/primitive-string")
    @Operation(summary = "GetPrimitiveString")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "200", description = "Returns the specified String."),
        @ApiResponse(responseCode = "404", description = "Can\'t find an String with the parameters provided.") })
    public ResponseEntity<String> GetPrimitiveString() {
        final String result = integrationService.GetPrimitiveString();

        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @GetMapping(path = "/primitive-int")
    @Operation(summary = "GetPrimitiveInt")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "200", description = "Returns the specified int."),
        @ApiResponse(responseCode = "404", description = "Can\'t find an int with the parameters provided.") })
    public ResponseEntity<Integer> GetPrimitiveInt() {
        final int result = integrationService.GetPrimitiveInt();

        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @GetMapping(path = "/primitive-string-list")
    @Operation(summary = "GetPrimitiveStringList")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "200", description = "Returns the specified String[].") })
    public ResponseEntity<String[]> GetPrimitiveStringList() {
        final String[] result = integrationService.GetPrimitiveStringList();

        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @GetMapping(path = "/invoice-op-with-return-type-wrapped")
    @Operation(summary = "GetInvoiceOpWithReturnTypeWrapped")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "200", description = "Returns the specified CustomDTO."),
        @ApiResponse(responseCode = "404", description = "Can\'t find an CustomDTO with the parameters provided.") })
    public ResponseEntity<CustomDTO> GetInvoiceOpWithReturnTypeWrapped() {
        final CustomDTO result = integrationService.GetInvoiceOpWithReturnTypeWrapped();
        if (result == null) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }

        return new ResponseEntity<>(result, HttpStatus.OK);
    }
}