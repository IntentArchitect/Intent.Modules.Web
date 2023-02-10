import { IsString } from 'class-validator';
import { ApiProperty } from '@nestjs/swagger';

export class PersonUpdateDTO {
  @IsString()
  @ApiProperty()
  name: string;
}