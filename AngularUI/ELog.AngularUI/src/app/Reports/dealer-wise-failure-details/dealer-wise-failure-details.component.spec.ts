import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DealerWiseFailureDetailsComponent } from './dealer-wise-failure-details.component';

describe('DealerWiseFailureDetailsComponent', () => {
  let component: DealerWiseFailureDetailsComponent;
  let fixture: ComponentFixture<DealerWiseFailureDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DealerWiseFailureDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DealerWiseFailureDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
