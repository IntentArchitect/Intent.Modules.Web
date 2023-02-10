import { Module, Logger } from '@nestjs/common';
import { AuthModule } from './auth/auth.module';
import { MiscController } from './web/rest/misc.controller';
import { PeopleController } from './web/rest/people.controller';
import { MiscService } from './services/misc.service';
import { PeopleService } from './services/people.service';
import { typeOrmConfig } from './orm.config';
import { TypeOrmExModule } from './typeorm/typeorm-ex.module';
import { PersonRepository } from './repository/person.repository';
import { UsersModules } from './users/users.modules';
import { IntentIgnore, IntentMerge } from './intent/intent.decorators';
import { ConfigModule } from '@nestjs/config';
import { TypeOrmModule } from '@nestjs/typeorm';

@IntentMerge()
@Module({
  imports: [
    ConfigModule.forRoot({ isGlobal: true }),
    AuthModule,
    TypeOrmModule.forRoot(typeOrmConfig),
    TypeOrmExModule.forCustomRepository([
      PersonRepository,
    ]),
    UsersModules
  ],
  controllers: [
    MiscController,
    PeopleController
  ],
  providers: [
    MiscService,
    PeopleService,
    Logger
  ]
})
export class AppModule { }