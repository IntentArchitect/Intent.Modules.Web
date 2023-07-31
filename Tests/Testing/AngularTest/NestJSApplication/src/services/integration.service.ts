import { Injectable } from '@nestjs/common';
import { CustomDTO } from './dto/Integration/custom.dto';
import { IntentIgnoreBody, IntentIgnore } from './../intent/intent.decorators';

@Injectable()
export class IntegrationService {

  @IntentIgnore()
  public readonly ReferenceNumber: string = "refnumber_1234";
  @IntentIgnore()
  public readonly DefaultString: string = "string value";
  @IntentIgnore()
  public readonly DefaultInt: number = 55;
  @IntentIgnore()
  public readonly ExceptionMessage: string = "Some exception message";
  @IntentIgnore()
  public readonly DefaultGuid: string = "b7698947-5237-4686-9571-442335426771";
  @IntentIgnore()
  public readonly Param1Value: string = "param 1";
  @IntentIgnore()
  public readonly Param2Value: number = 42;

  //@IntentCanAdd()
  constructor() {}

  @IntentIgnoreBody()
  async queryParamOp(param1: string, param2: number): Promise<CustomDTO> {
    if (param1 != this.Param1Value) {
      throw new Error(`${"param1"} is not "${this.Param1Value}" but is "${param1}"`);
    }
    if (param2 != this.Param2Value) {
      throw new Error(`${"param2"} is not "${this.Param2Value}" but is "${param2}"`);
    }
    return {
      referenceNumber: this.ReferenceNumber
    };
  }

  @IntentIgnoreBody()
  async headerParamOp(param1: string): Promise<void> {
    if (param1 != this.Param1Value) {
      throw new Error(`${"param1"} is not "${this.Param1Value}" but is "${param1}"`);
    }
  }

  @IntentIgnoreBody()
  async routeParamOp(param1: string): Promise<void> {
    if (param1 != this.Param1Value) {
      throw new Error(`${"param1"} is not "${this.Param1Value}" but is "${param1}"`);
    }
  }

  @IntentIgnoreBody()
  async bodyParamOp(param1: CustomDTO): Promise<void> {
    if (param1.referenceNumber != this.ReferenceNumber) {
      throw new Error(`${"param1.referenceNumber"} is not "${this.ReferenceNumber}" but is "${param1.referenceNumber}"`);
    }
  }

  @IntentIgnoreBody()
  async throwsException(): Promise<void> {
    throw new Error(this.ExceptionMessage);
  }

  @IntentIgnoreBody()
  async getWrappedPrimitiveGuid(): Promise<string> {
    return this.DefaultGuid;
  }

  @IntentIgnoreBody()
  async getWrappedPrimitiveString(): Promise<string> {
    return this.DefaultString;
  }

  @IntentIgnoreBody()
  async getWrappedPrimitiveInt(): Promise<number> {
    return this.DefaultInt;
  }

  @IntentIgnoreBody()
  async getPrimitiveGuid(): Promise<string> {
    return this.DefaultGuid;
  }

  @IntentIgnoreBody()
  async getPrimitiveString(): Promise<string> {
    return this.DefaultString;
  }

  @IntentIgnoreBody()
  async getPrimitiveInt(): Promise<number> {
    return this.DefaultInt;
  }

  @IntentIgnoreBody()
  async getPrimitiveStringList(): Promise<string[]> {
    return [this.DefaultString];
  }

  @IntentIgnoreBody()
  async nonHttpSettingsOperation(): Promise<void> {
    
  }

  @IntentIgnoreBody()
  async getInvoiceOpWithReturnTypeWrapped(): Promise<CustomDTO> {
    return {
      referenceNumber: this.ReferenceNumber
    };
  }
}
