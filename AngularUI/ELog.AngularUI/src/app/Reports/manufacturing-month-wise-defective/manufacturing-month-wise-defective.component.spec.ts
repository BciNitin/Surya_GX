import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManufacturingMonthWiseDefectiveComponent } from './manufacturing-month-wise-defective.component';

describe('ManufacturingMonthWiseDefectiveComponent', () => {
  let component: ManufacturingMonthWiseDefectiveComponent;
  let fixture: ComponentFixture<ManufacturingMonthWiseDefectiveComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManufacturingMonthWiseDefectiveComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManufacturingMonthWiseDefectiveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
