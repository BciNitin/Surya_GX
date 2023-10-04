import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QualitySamplingComponent } from './quality-sampling.component';

describe('QualitySamplingComponent', () => {
  let component: QualitySamplingComponent;
  let fixture: ComponentFixture<QualitySamplingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QualitySamplingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QualitySamplingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
