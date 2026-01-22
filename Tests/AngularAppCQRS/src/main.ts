import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { MainLayout } from './app/layout/main/main.component';


bootstrapApplication(MainLayout, appConfig)
  .catch((err) => console.error(err));
