import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GrnConfirmationComponent } from './grn-confirmation.component';

describe('GrnConfirmationComponent', () => {
  let component: GrnConfirmationComponent;
  let fixture: ComponentFixture<GrnConfirmationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GrnConfirmationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GrnConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
