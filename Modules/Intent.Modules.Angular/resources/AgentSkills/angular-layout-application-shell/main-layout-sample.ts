import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { IntentIgnoreBody, IntentMerge, IntentIgnore } from './../../intent/intent.decorators';
import { Component, OnInit } from '@angular/core';
import { RouterOutlet, RouterLink } from '@angular/router';

@IntentMerge()
@Component({
  selector: 'app-main',
  standalone: true,
  templateUrl: 'main.component.html',
  styleUrls: ['main.component.scss'],
  imports: [
    RouterOutlet,    
    RouterLink,    
    MatSidenavModule,    
    MatToolbarModule,    
    MatIconModule,    
    MatButtonModule,    
    MatListModule,
  ],
})
export class MainLayout {
}