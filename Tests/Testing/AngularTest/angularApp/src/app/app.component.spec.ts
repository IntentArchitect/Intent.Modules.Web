import { TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';

describe('AppComponent', () => {
  beforeEach(() => TestBed.configureTestingModule({
    declarations: [AppComponent]
  }));


  it('should invoke backend', async () => {
    const fixture = TestBed.createComponent(AppComponent);
    let observables = fixture.componentInstance.performServiceCalls();
    let done = false;
    observables.subscribe(res => {
      console.log(res);
      done = true;
    });
    while(!done){
      
    }
  });
});
