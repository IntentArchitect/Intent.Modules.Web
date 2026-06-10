import { Module, Logger } from '@nestjs/common';
import { AuthModule } from './auth/auth.module';
import { IntegrationController } from './web/rest/integration.controller';
import { IntegrationService } from './services/integration.service';
import { typeOrmConfig } from './orm.config';
import { BasicAuditingSubscriber } from './typeorm/basic-auditing-subscriber';
import { UsersModules } from './users/users.modules';
import { ConfigModule } from '@nestjs/config';
import { TypeOrmModule } from '@nestjs/typeorm';
import { ClsModule } from 'nestjs-cls';
import { IntentMerge } from './intent/intent.decorators';

@IntentMerge()
@Module({
    imports: [
        ConfigModule.forRoot({ isGlobal: true }),
        AuthModule,
        UsersModules,
        ClsModule.forRoot({
            global: true,
            middleware: { mount: true },
        }),
        TypeOrmModule.forRoot(typeOrmConfig)
    ],
    controllers: [
        IntegrationController
    ],
    providers: [
        Logger,
        IntegrationService,
        BasicAuditingSubscriber
    ]
})
export class AppModule { }
