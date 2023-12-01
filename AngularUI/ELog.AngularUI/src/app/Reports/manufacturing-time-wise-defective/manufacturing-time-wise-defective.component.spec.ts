import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManufacturingTimeWiseDefectiveComponent } from './manufacturing-time-wise-defective.component';

describe('ManufacturingTimeWiseDefectiveComponent', () => {
  let component: ManufacturingTimeWiseDefectiveComponent;
  let fixture: ComponentFixture<ManufacturingTimeWiseDefectiveComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManufacturingTimeWiseDefectiveComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManufacturingTimeWiseDefectiveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
