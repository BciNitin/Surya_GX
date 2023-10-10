import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QualityConfirmationComponent } from './quality-confirmation.component';

describe('QualityConfirmationComponent', () => {
  let component: QualityConfirmationComponent;
  let fixture: ComponentFixture<QualityConfirmationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QualityConfirmationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QualityConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
