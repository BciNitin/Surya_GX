import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LifeCycleReportComponent } from './life-cycle-report.component';

describe('LifeCycleReportComponent', () => {
  let component: LifeCycleReportComponent;
  let fixture: ComponentFixture<LifeCycleReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LifeCycleReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LifeCycleReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
