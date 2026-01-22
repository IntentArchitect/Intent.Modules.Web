package com.AngularTest.JavaApplication.application.services;

import com.AngularTest.JavaApplication.application.models.Integration.CustomDTO;
import java.util.UUID;
import com.AngularTest.JavaApplication.exceptions.Integration.TestException;

public interface IntegrationService {
    CustomDTO QueryParamOp(String param1, int param2);

    void HeaderParamOp(String param1);

    void RouteParamOp(String param1);

    void BodyParamOp(CustomDTO param1);

    void ThrowsException()
            throws TestException;

    UUID GetWrappedPrimitiveGuid();

    String GetWrappedPrimitiveString();

    int GetWrappedPrimitiveInt();

    UUID GetPrimitiveGuid();

    String GetPrimitiveString();

    int GetPrimitiveInt();

    String[] GetPrimitiveStringList();

    void NonHttpSettingsOperation();

    CustomDTO GetInvoiceOpWithReturnTypeWrapped();

}