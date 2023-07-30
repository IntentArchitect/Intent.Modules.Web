import { Component, OnInit } from '@angular/core';
import { IntentIgnore } from './intent/intent.decorators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  isCollapsed: boolean = false;

  //@IntentCanAdd()
  constructor() { }

  @IntentIgnore()
  ngOnInit() {
  }
}
