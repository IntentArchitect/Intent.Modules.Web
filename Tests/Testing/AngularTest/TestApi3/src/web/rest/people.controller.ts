import { Controller, Logger, Post, Req, Request, Body, Get, Param, Put, Delete, Query, Headers } from '@nestjs/common';
import { PersonCreateDTO } from './../../services/dto/People/person-create.dto';
import { PersonDTO } from './../../services/dto/People/person.dto';
import { PersonUpdateDTO } from './../../services/dto/People/person-update.dto';
import { PeopleService } from './../../services/people.service';
import { JsonResponse } from './json-response';
import { ApiTags, ApiCreatedResponse, ApiBadRequestResponse, ApiOkResponse, ApiNotFoundResponse, ApiNoContentResponse } from '@nestjs/swagger';

@ApiTags('People')
@Controller('api/people')
export class PeopleController {
  logger = new Logger('PeopleController');

  constructor(private readonly peopleService: PeopleService) {}

  @Post('')
  @ApiCreatedResponse({
    description: 'The record has been successfully created.',
    type: 'string',
  })
  @ApiBadRequestResponse({ description: 'Bad request.' })
  async create(@Req() req: Request, @Body() dto: PersonCreateDTO): Promise<string> {
    const result = await this.peopleService.create(dto);
    return result;
  }

  @Get(':id')
  @ApiOkResponse({
    description: 'Result retrieved successfully.',
    type: PersonDTO,
  })
  @ApiBadRequestResponse({ description: 'Bad request.' })
  @ApiNotFoundResponse({ description: 'Response not found.' })
  async findById(@Req() req: Request, @Param('id') id: string): Promise<PersonDTO> {
    const result = await this.peopleService.findById(id);
    return result;
  }

  @Get('')
  @ApiOkResponse({
    description: 'Result retrieved successfully.',
    type: PersonDTO,
    isArray: true,
  })
  async findAll(@Req() req: Request): Promise<PersonDTO[]> {
    const result = await this.peopleService.findAll();
    return result;
  }

  @Put(':id')
  @ApiNoContentResponse({
    description: 'Successfully updated.',
  })
  @ApiBadRequestResponse({ description: 'Bad request.' })
  async update(@Req() req: Request, @Param('id') id: string, @Body() dto: PersonUpdateDTO): Promise<void> {
    return await this.peopleService.update(id, dto);
  }

  @Delete(':id')
  @ApiOkResponse({
    description: 'Successfully deleted.',
  })
  @ApiBadRequestResponse({ description: 'Bad request.' })
  async delete(@Req() req: Request, @Param('id') id: string): Promise<void> {
    return await this.peopleService.delete(id);
  }
}