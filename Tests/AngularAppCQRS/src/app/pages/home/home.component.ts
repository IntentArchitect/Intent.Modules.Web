//@IntentMerge()
import { IntentIgnoreBody, IntentMerge, IntentIgnore } from './../../intent/intent.decorators';
import { Component, OnInit } from '@angular/core';

@IntentMerge()
@Component({
  selector: 'app-home',
  standalone: true,
  templateUrl: 'home.component.html',
  styleUrls: ['home.component.scss'],
})
export class HomeComponent implements OnInit {
  //@IntentMerge()
  constructor() {
  }

  @IntentMerge()
  ngOnInit(): void {
  }
}