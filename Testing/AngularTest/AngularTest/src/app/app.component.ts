import { Component, OnInit } from '@angular/core';
import { IntentIgnore, IntentManage } from './../intent.decorators';
import { from } from 'rxjs';
import { concatAll } from 'rxjs/operators';
import { TestPeopleClient } from './http-client/test-people-client.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  @IntentIgnore()
  isCollapsed: boolean = true;

  //@IntentCanAdd()
  constructor(private client: TestPeopleClient) { }

  @IntentIgnore()
  ngOnInit() {

  }

  @IntentIgnore()
  performServiceCalls(): void {
    this.client.create({ name: "John Doe" })
      .subscribe(res => {
        let calls = [
          this.client.findAll(),
          this.client.findById(res),
          this.client.delete(res),
          this.client.getWithQueryParam(res),
          this.client.getWithRouteParam(res),
          this.client.postWithBodyParam({ name: "Update name" }),
          this.client.postWithFormParam("param 1 value", "param 2 value"),
          this.client.postWithHeaderParam("header param value"),
          this.client.getWithPrimitiveResultInt(),
          this.client.getWithPrimitiveResultBool(),
          this.client.getWithPrimitiveResultStr(),
          this.client.getWithPrimitiveResultDate(),
          this.client.getWithPrimitiveResultWrapInt(),
          this.client.getWithPrimitiveResultWrapBool(),
          this.client.getWithPrimitiveResultWrapStr(),
          this.client.getWithPrimitiveResultWrapDate()
        ];
        from(calls)
          .pipe(concatAll())
          .subscribe(res => {
            console.log(res);
          });
      });
  
  }
}
