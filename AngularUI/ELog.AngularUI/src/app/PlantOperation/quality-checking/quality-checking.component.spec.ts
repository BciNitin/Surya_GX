import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QualityCheckingComponent } from './quality-checking.component';

describe('QualityCheckingComponent', () => {
  let component: QualityCheckingComponent;
  let fixture: ComponentFixture<QualityCheckingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QualityCheckingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QualityCheckingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
