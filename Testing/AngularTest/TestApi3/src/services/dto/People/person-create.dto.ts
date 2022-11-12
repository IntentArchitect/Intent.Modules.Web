import { IsString } from 'class-validator';
import { ApiProperty } from '@nestjs/swagger';

export class PersonCreateDTO {
  @IsString()
  @ApiProperty()
  name: string;
}