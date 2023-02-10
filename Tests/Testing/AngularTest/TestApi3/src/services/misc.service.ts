import { Injectable } from '@nestjs/common';
import { PersonDTO } from './dto/People/person.dto';
import { PersonUpdateDTO } from './dto/People/person-update.dto';
import { DateDTO } from './dto/Misc/date.dto';
import { IntentIgnoreBody } from './../intent/intent.decorators';
import { randomUUID } from 'crypto';

@Injectable()
export class MiscService {

  //@IntentCanAdd()
  constructor() { }

  @IntentIgnoreBody()
  async getWithQueryParam(idParam: string): Promise<PersonDTO> {
    return {
      id: randomUUID(),
      name: 'Test ' + randomUUID()
    };
  }

  @IntentIgnoreBody()
  async getWithRouteParam(routeId: string): Promise<PersonDTO> {
    return {
      id: randomUUID(),
      name: 'Test ' + randomUUID()
    };
  }

  @IntentIgnoreBody()
  async postWithHeaderParam(param: string): Promise<void> {
    
  }

  @IntentIgnoreBody()
  async postWithBodyParam(param: PersonUpdateDTO): Promise<void> {
    
  }

  @IntentIgnoreBody()
  async getWithPrimitiveResultInt(): Promise<number> {
    return 12;
  }

  @IntentIgnoreBody()
  async getWithPrimitiveResultWrapInt(): Promise<number> {
    return 13;
  }

  @IntentIgnoreBody()
  async getWithPrimitiveResultBool(): Promise<boolean> {
    return true;
  }

  @IntentIgnoreBody()
  async getWithPrimitiveResultWrapBool(): Promise<boolean> {
    return false;
  }

  @IntentIgnoreBody()
  async getWithPrimitiveResultStr(): Promise<string> {
    return "Primitive string value";
  }

  @IntentIgnoreBody()
  async getWithPrimitiveResultWrapStr(): Promise<string> {
    return "Wrapped Primitive string value";
  }

  @IntentIgnoreBody()
  async getWithPrimitiveResultDate(): Promise<Date> {
    return new Date();
  }

  @IntentIgnoreBody()
  async getWithPrimitiveResultWrapDate(): Promise<Date> {
    return new Date();
  }

  @IntentIgnoreBody()
  async postDateParam(date: Date, datetime: Date): Promise<void> {
    
  }

  @IntentIgnoreBody()
  async postDateParamDto(dto: DateDTO): Promise<void> {
    
  }
}
