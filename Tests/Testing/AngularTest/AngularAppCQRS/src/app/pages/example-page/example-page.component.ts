//@IntentMerge()
import { IntentIgnoreBody, IntentMerge, IntentIgnore } from './../../intent/intent.decorators';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@IntentMerge()
@Component({
  selector: 'app-example-page',
  standalone: true,
  templateUrl: 'example-page.component.html',
  styleUrls: ['example-page.component.scss'],
})
export class ExamplePageComponent implements OnInit {
  title: string = '';

  //@IntentMerge()
  constructor(private route: ActivatedRoute) {
  }

  @IntentMerge()
  ngOnInit(): void {
    const title = this.route.snapshot.paramMap.get('title');
    if (!title) {
      throw new Error("Expected 'title' not supplied");
    }
    this.title = title;
  }
}