import { Component, OnInit } from '@angular/core';
import { IntentIgnore, IntentManage } from './../intent.decorators';
import { from } from 'rxjs';
import { concatAll } from 'rxjs/operators';
import { TestPeopleClient } from './http-client/test-people-client.service';
import { MiscClient } from './http-client/misc-client.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  @IntentIgnore()
  isCollapsed: boolean = true;

  //@IntentCanAdd()
  constructor(private testPeopleClient: TestPeopleClient,
    private miscClient: MiscClient) { }

  @IntentIgnore()
  ngOnInit() {

  }

  @IntentIgnore()
  performServiceCalls(): void {
    this.testPeopleClient.create({ name: "John Doe" })
      .subscribe(res => {
        let calls = [
          this.testPeopleClient.findAll(),
          this.testPeopleClient.findById(res),
          this.testPeopleClient.delete(res),
          this.miscClient.getWithQueryParam(res),
          this.miscClient.getWithRouteParam(res),
          this.miscClient.postWithBodyParam({ name: "Update name" }),
          this.miscClient.postWithHeaderParam("header param value"),
          this.miscClient.getWithPrimitiveResultInt(),
          this.miscClient.getWithPrimitiveResultBool(),
          this.miscClient.getWithPrimitiveResultStr(),
          this.miscClient.getWithPrimitiveResultDate(),
          this.miscClient.getWithPrimitiveResultWrapInt(),
          this.miscClient.getWithPrimitiveResultWrapBool(),
          this.miscClient.getWithPrimitiveResultWrapStr(),
          this.miscClient.getWithPrimitiveResultWrapDate(),
          this.miscClient.postDateParam(new Date(), new Date()),
          this.miscClient.postDateParamDto({ date: new Date(), dateTime: new Date() })
        ];
        from(calls)
          .pipe(concatAll())
          .subscribe(res => {
            console.log(res);
          });
      });

  }
}
