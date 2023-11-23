import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RevalidationReportComponent } from './revalidation-report.component';

describe('RevalidationReportComponent', () => {
  let component: RevalidationReportComponent;
  let fixture: ComponentFixture<RevalidationReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RevalidationReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RevalidationReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
