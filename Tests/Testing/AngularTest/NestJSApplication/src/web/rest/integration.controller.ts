import { Controller, Logger, Get, Req, Request, Query, Post, Headers, Param, Body, UseInterceptors } from '@nestjs/common';
import { CustomDTO } from './../../services/dto/Integration/custom.dto';
import { IntegrationService } from './../../services/integration.service';
import { JsonResponse } from './json-response';
import { ApiTags, ApiOkResponse, ApiBadRequestResponse, ApiNotFoundResponse, ApiCreatedResponse } from '@nestjs/swagger';
import { FileFieldsInterceptor } from '@nestjs/platform-express';

@ApiTags('Integration')
@Controller('api/integration')
export class IntegrationController {
  logger = new Logger('IntegrationController');

  constructor(private readonly integrationService: IntegrationService) {}

  @Get('query-param-op')
  @ApiOkResponse({
    description: 'Result retrieved successfully.',
    type: CustomDTO,
  })
  @ApiBadRequestResponse({ description: 'Bad request.' })
  @ApiNotFoundResponse({ description: 'Response not found.' })
  async queryParamOp(@Req() req: Request, @Query('param1') param1: string, @Query('param2') param2: number): Promise<CustomDTO> {
    const result = await this.integrationService.queryParamOp(param1, param2);
    return result;
  }

  @Post('header-param-op')
  @ApiCreatedResponse({
    description: 'The record has been successfully created.',
  })
  @ApiBadRequestResponse({ description: 'Bad request.' })
  async headerParamOp(@Req() req: Request, @Headers('MY-HEADER') param1: string): Promise<void> {
    return await this.integrationService.headerParamOp(param1);
  }

  @Post('route-param-op/:param1')
  @ApiCreatedResponse({
    description: 'The record has been successfully created.',
  })
  @ApiBadRequestResponse({ description: 'Bad request.' })
  async routeParamOp(@Req() req: Request, @Param('param1') param1: string): Promise<void> {
    return await this.integrationService.routeParamOp(param1);
  }

  @Post('body-param-op')
  @ApiCreatedResponse({
    description: 'The record has been successfully created.',
  })
  @ApiBadRequestResponse({ description: 'Bad request.' })
  async bodyParamOp(@Req() req: Request, @Body() param1: CustomDTO): Promise<void> {
    return await this.integrationService.bodyParamOp(param1);
  }

  @Post('throws-exception')
  @ApiCreatedResponse({
    description: 'The record has been successfully created.',
  })
  async throwsException(@Req() req: Request): Promise<void> {
    return await this.integrationService.throwsException();
  }

  @Get('wrapped-primitive-guid')
  @ApiOkResponse({
    description: 'Result retrieved successfully.',
    type: String,
  })
  @ApiNotFoundResponse({ description: 'Response not found.' })
  async getWrappedPrimitiveGuid(@Req() req: Request): Promise<JsonResponse<string>> {
    const result = await this.integrationService.getWrappedPrimitiveGuid();
    return new JsonResponse<string>(result);
  }

  @Get('wrapped-primitive-string')
  @ApiOkResponse({
    description: 'Result retrieved successfully.',
    type: String,
  })
  @ApiNotFoundResponse({ description: 'Response not found.' })
  async getWrappedPrimitiveString(@Req() req: Request): Promise<JsonResponse<string>> {
    const result = await this.integrationService.getWrappedPrimitiveString();
    return new JsonResponse<string>(result);
  }

  @Get('wrapped-primitive-int')
  @ApiOkResponse({
    description: 'Result retrieved successfully.',
    type: 'number',
  })
  @ApiNotFoundResponse({ description: 'Response not found.' })
  async getWrappedPrimitiveInt(@Req() req: Request): Promise<JsonResponse<number>> {
    const result = await this.integrationService.getWrappedPrimitiveInt();
    return new JsonResponse<number>(result);
  }

  @Get('primitive-guid')
  @ApiOkResponse({
    description: 'Result retrieved successfully.',
    type: String,
  })
  @ApiNotFoundResponse({ description: 'Response not found.' })
  async getPrimitiveGuid(@Req() req: Request): Promise<string> {
    const result = await this.integrationService.getPrimitiveGuid();
    return result;
  }

  @Get('primitive-string')
  @ApiOkResponse({
    description: 'Result retrieved successfully.',
    type: String,
  })
  @ApiNotFoundResponse({ description: 'Response not found.' })
  async getPrimitiveString(@Req() req: Request): Promise<string> {
    const result = await this.integrationService.getPrimitiveString();
    return result;
  }

  @Get('primitive-int')
  @ApiOkResponse({
    description: 'Result retrieved successfully.',
    type: 'number',
  })
  @ApiNotFoundResponse({ description: 'Response not found.' })
  async getPrimitiveInt(@Req() req: Request): Promise<number> {
    const result = await this.integrationService.getPrimitiveInt();
    return result;
  }

  @Get('primitive-string-list')
  @ApiOkResponse({
    description: 'Result retrieved successfully.',
    type: 'String',
    isArray: true,
  })
  async getPrimitiveStringList(@Req() req: Request): Promise<string[]> {
    const result = await this.integrationService.getPrimitiveStringList();
    return result;
  }

  @Get('invoice-op-with-return-type-wrapped')
  @ApiOkResponse({
    description: 'Result retrieved successfully.',
    type: CustomDTO,
  })
  @ApiNotFoundResponse({ description: 'Response not found.' })
  async getInvoiceOpWithReturnTypeWrapped(@Req() req: Request): Promise<CustomDTO> {
    const result = await this.integrationService.getInvoiceOpWithReturnTypeWrapped();
    return result;
  }
}