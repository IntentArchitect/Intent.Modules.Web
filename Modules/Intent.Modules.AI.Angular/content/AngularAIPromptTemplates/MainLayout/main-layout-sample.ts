import { RouterLink, RouterOutlet } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { OnInit, Component } from '@angular/core';
import { IntentIgnore, IntentIgnoreBody, IntentMerge } from './../../intent/intent.decorators';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';

@IntentMerge()
@Component({
  selector: 'app-main',
  standalone: true,
  templateUrl: 'main.component.html',
  styleUrls: ['main.component.scss'],
  imports: [    
    MatListModule,    
    MatButtonModule,    
    MatIconModule,    
    MatToolbarModule,
    RouterOutlet,    
    RouterLink,    
    MatSidenavModule
  ],
})
export class MainLayout {
}
