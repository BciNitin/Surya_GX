import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PackingOrderConfirmationComponent } from './packing-order-confirmation.component';

describe('PackingOrderConfirmationComponent', () => {
  let component: PackingOrderConfirmationComponent;
  let fixture: ComponentFixture<PackingOrderConfirmationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PackingOrderConfirmationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PackingOrderConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
