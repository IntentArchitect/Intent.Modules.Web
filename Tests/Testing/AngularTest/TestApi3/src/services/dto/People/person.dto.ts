import { ApiProperty } from '@nestjs/swagger';
import { IsString } from 'class-validator';
import { Person } from 'src/domain/entities/person.entity';

export class PersonDTO {
  @ApiProperty()
  id: string;

  @IsString()
  @ApiProperty()
  name: string;

  static fromPerson(person: Person) {
    if (person == null) {
      return null;
    }
    const dto = new PersonDTO();
    dto.name = person.name;
    return dto;
  }

  static requiredRelations: string[] = [];
}