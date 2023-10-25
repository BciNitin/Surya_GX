import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QualityTestedItemplacementComponent } from './quality-tested-itemplacement.component';

describe('QualityTestedItemplacementComponent', () => {
  let component: QualityTestedItemplacementComponent;
  let fixture: ComponentFixture<QualityTestedItemplacementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QualityTestedItemplacementComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QualityTestedItemplacementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
