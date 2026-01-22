package com.AngularTest.JavaApplication.application.services.impl;
import com.AngularTest.JavaApplication.exceptions.Integration.TestException;

import lombok.AllArgsConstructor;
import org.springframework.stereotype.Service;
import com.AngularTest.JavaApplication.application.models.Integration.CustomDTO;
import com.AngularTest.JavaApplication.application.services.IntegrationService;
import com.AngularTest.JavaApplication.intent.IntentIgnoreBody;
import com.AngularTest.JavaApplication.intent.IntentMerge;
import java.util.UUID;
import org.springframework.transaction.annotation.Transactional;

@Service
@AllArgsConstructor
@IntentMerge
public class IntegrationServiceImpl implements IntegrationService {
    public final String ReferenceNumber = "refnumber_1234";
    public final String ExceptionMessage = "Some exception message";
    public final String DefaultString = "string value";
    public static final UUID DefaultGuid = UUID.fromString("b7698947-5237-4686-9571-442335426771");
    public final int DefaultInt = 55;
    public final String Param1Value = "param 1";
    public final int Param2Value = 42;

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public CustomDTO QueryParamOp(String param1, int param2) {
        if (!param1.equals(this.Param1Value)) {
            throw new IllegalArgumentException("%s is not \"%s\" but is \"%s\"".formatted("param1", Param1Value, param1));
        }
        if (param2 != this.Param2Value) {
            throw new IllegalArgumentException("%s is not \"%x\" but is \"%x\"".formatted("param2", Param2Value, param2));
        }
        return new CustomDTO(this.ReferenceNumber);
    }

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void HeaderParamOp(String param1) {
        if (!param1.equals(this.Param1Value)) {
            throw new IllegalArgumentException("%s is not \"%s\" but is \"%s\"".formatted("param1", Param1Value, param1));
        }
    }

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void RouteParamOp(String param1) {
        if (!param1.equals(this.Param1Value)) {
            throw new IllegalArgumentException("%s is not \"%s\" but is \"%s\"".formatted("param1", Param1Value, param1));
        }
    }

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void BodyParamOp(CustomDTO param1) {
        if (!param1.getReferenceNumber().equals(this.ReferenceNumber)) {
            throw new IllegalArgumentException("%s is not \"%s\" but is \"%s\"".formatted("param1.getReferenceNumber()", this.ReferenceNumber, param1.getReferenceNumber()));
        }
    }

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void ThrowsException() throws TestException {
        throw new TestException();
    }

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public UUID GetWrappedPrimitiveGuid() {
        return DefaultGuid;
    }

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public String GetWrappedPrimitiveString() {
        return DefaultString;
    }

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public int GetWrappedPrimitiveInt() {
        return DefaultInt;
    }

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public UUID GetPrimitiveGuid() {
        return DefaultGuid;
    }

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public String GetPrimitiveString() {
        return DefaultString;
    }

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public int GetPrimitiveInt() {
        return DefaultInt;
    }

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public String[] GetPrimitiveStringList() {
        return new String[]{ DefaultString };
    }

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void NonHttpSettingsOperation() {

    }

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public CustomDTO GetInvoiceOpWithReturnTypeWrapped() {
        return new CustomDTO(this.ReferenceNumber);
    }
}