import { IsDate } from 'class-validator';
import { ApiProperty } from '@nestjs/swagger';
import { Type } from 'class-transformer';

export class DateDTO {
  @IsDate()
  @ApiProperty()
  @Type(() => Date)
  date: Date;

  @IsDate()
  @ApiProperty()
  @Type(() => Date)
  dateTime: Date;
}