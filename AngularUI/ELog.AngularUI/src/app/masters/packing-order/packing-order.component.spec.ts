import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PackingOrderComponent } from './packing-order.component';

describe('PackingOrderComponent', () => {
  let component: PackingOrderComponent;
  let fixture: ComponentFixture<PackingOrderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PackingOrderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PackingOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
