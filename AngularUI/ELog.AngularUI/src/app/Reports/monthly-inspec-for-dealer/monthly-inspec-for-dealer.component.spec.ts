import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MonthlyInspecForDealerComponent } from './monthly-inspec-for-dealer.component';

describe('MonthlyInspecForDealerComponent', () => {
  let component: MonthlyInspecForDealerComponent;
  let fixture: ComponentFixture<MonthlyInspecForDealerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MonthlyInspecForDealerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MonthlyInspecForDealerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
