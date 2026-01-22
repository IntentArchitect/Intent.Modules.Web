import { IsString } from 'class-validator';
import { ApiProperty } from '@nestjs/swagger';

export class CustomDTO {
  @IsString()
  @ApiProperty()
  referenceNumber: string;
}