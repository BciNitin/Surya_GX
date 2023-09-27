import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LineWorkCenterComponent } from './line-work-center.component';

describe('LineWorkCenterComponent', () => {
  let component: LineWorkCenterComponent;
  let fixture: ComponentFixture<LineWorkCenterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LineWorkCenterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LineWorkCenterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
