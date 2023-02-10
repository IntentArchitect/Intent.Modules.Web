import { Controller, Logger, Get, Req, Request, Query, Param, Post, Headers, Body } from '@nestjs/common';
import { PersonDTO } from './../../services/dto/People/person.dto';
import { PersonUpdateDTO } from './../../services/dto/People/person-update.dto';
import { DateDTO } from './../../services/dto/Misc/date.dto';
import { MiscService } from './../../services/misc.service';
import { JsonResponse } from './json-response';
import { ApiTags, ApiOkResponse, ApiBadRequestResponse, ApiNotFoundResponse, ApiCreatedResponse } from '@nestjs/swagger';

@ApiTags('Misc')
@Controller('api/misc')
export class MiscController {
  logger = new Logger('MiscController');

  constructor(private readonly miscService: MiscService) {}

  @Get('getwithqueryparam')
  @ApiOkResponse({
    description: 'Result retrieved successfully.',
    type: PersonDTO,
  })
  @ApiBadRequestResponse({ description: 'Bad request.' })
  @ApiNotFoundResponse({ description: 'Response not found.' })
  async getWithQueryParam(@Req() req: Request, @Query('idParam') idParam: string): Promise<PersonDTO> {
    const result = await this.miscService.getWithQueryParam(idParam);
    return result;
  }

  @Get('getwithrouteparam/:routeId')
  @ApiOkResponse({
    description: 'Result retrieved successfully.',
    type: PersonDTO,
  })
  @ApiBadRequestResponse({ description: 'Bad request.' })
  @ApiNotFoundResponse({ description: 'Response not found.' })
  async getWithRouteParam(@Req() req: Request, @Param('routeId') routeId: string): Promise<PersonDTO> {
    const result = await this.miscService.getWithRouteParam(routeId);
    return result;
  }

  @Post('postwithheaderparam')
  @ApiCreatedResponse({
    description: 'The record has been successfully created.',
  })
  @ApiBadRequestResponse({ description: 'Bad request.' })
  async postWithHeaderParam(@Req() req: Request, @Headers('param') param: string): Promise<void> {
    return await this.miscService.postWithHeaderParam(param);
  }

  @Post('postwithbodyparam')
  @ApiCreatedResponse({
    description: 'The record has been successfully created.',
  })
  @ApiBadRequestResponse({ description: 'Bad request.' })
  async postWithBodyParam(@Req() req: Request, @Body() param: PersonUpdateDTO): Promise<void> {
    return await this.miscService.postWithBodyParam(param);
  }

  @Get('getwithprimitiveresultint')
  @ApiOkResponse({
    description: 'Result retrieved successfully.',
    type: 'number',
  })
  @ApiNotFoundResponse({ description: 'Response not found.' })
  async getWithPrimitiveResultInt(@Req() req: Request): Promise<number> {
    const result = await this.miscService.getWithPrimitiveResultInt();
    return result;
  }

  @Get('getwithprimitiveresultwrapint')
  @ApiOkResponse({
    description: 'Result retrieved successfully.',
    type: 'number',
  })
  @ApiNotFoundResponse({ description: 'Response not found.' })
  async getWithPrimitiveResultWrapInt(@Req() req: Request): Promise<JsonResponse<number>> {
    const result = await this.miscService.getWithPrimitiveResultWrapInt();
    return new JsonResponse<number>(result);
  }

  @Get('getwithprimitiveresultbool')
  @ApiOkResponse({
    description: 'Result retrieved successfully.',
    type: 'boolean',
  })
  @ApiNotFoundResponse({ description: 'Response not found.' })
  async getWithPrimitiveResultBool(@Req() req: Request): Promise<boolean> {
    const result = await this.miscService.getWithPrimitiveResultBool();
    return result;
  }

  @Get('getwithprimitiveresultwrapbool')
  @ApiOkResponse({
    description: 'Result retrieved successfully.',
    type: 'boolean',
  })
  @ApiNotFoundResponse({ description: 'Response not found.' })
  async getWithPrimitiveResultWrapBool(@Req() req: Request): Promise<JsonResponse<boolean>> {
    const result = await this.miscService.getWithPrimitiveResultWrapBool();
    return new JsonResponse<boolean>(result);
  }

  @Get('getwithprimitiveresultstr')
  @ApiOkResponse({
    description: 'Result retrieved successfully.',
    type: 'string',
  })
  @ApiNotFoundResponse({ description: 'Response not found.' })
  async getWithPrimitiveResultStr(@Req() req: Request): Promise<string> {
    const result = await this.miscService.getWithPrimitiveResultStr();
    return result;
  }

  @Get('getwithprimitiveresultwrapstr')
  @ApiOkResponse({
    description: 'Result retrieved successfully.',
    type: 'string',
  })
  @ApiNotFoundResponse({ description: 'Response not found.' })
  async getWithPrimitiveResultWrapStr(@Req() req: Request): Promise<JsonResponse<string>> {
    const result = await this.miscService.getWithPrimitiveResultWrapStr();
    return new JsonResponse<string>(result);
  }

  @Get('getwithprimitiveresultdate')
  @ApiOkResponse({
    description: 'Result retrieved successfully.',
    type: 'Date',
  })
  @ApiNotFoundResponse({ description: 'Response not found.' })
  async getWithPrimitiveResultDate(@Req() req: Request): Promise<Date> {
    const result = await this.miscService.getWithPrimitiveResultDate();
    return result;
  }

  @Get('getwithprimitiveresultwrapdate')
  @ApiOkResponse({
    description: 'Result retrieved successfully.',
    type: 'Date',
  })
  @ApiNotFoundResponse({ description: 'Response not found.' })
  async getWithPrimitiveResultWrapDate(@Req() req: Request): Promise<JsonResponse<Date>> {
    const result = await this.miscService.getWithPrimitiveResultWrapDate();
    return new JsonResponse<Date>(result);
  }

  @Post('postdateparam')
  @ApiCreatedResponse({
    description: 'The record has been successfully created.',
  })
  @ApiBadRequestResponse({ description: 'Bad request.' })
  async postDateParam(@Req() req: Request, @Query('date') date: Date, @Query('datetime') datetime: Date): Promise<void> {
    return await this.miscService.postDateParam(date, datetime);
  }

  @Post('postdateparamdto')
  @ApiCreatedResponse({
    description: 'The record has been successfully created.',
  })
  @ApiBadRequestResponse({ description: 'Bad request.' })
  async postDateParamDto(@Req() req: Request, @Body() dto: DateDTO): Promise<void> {
    return await this.miscService.postDateParamDto(dto);
  }
}