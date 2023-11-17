import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AsOnDateInventoryComponent } from './as-on-date-inventory.component';

describe('AsOnDateInventoryComponent', () => {
  let component: AsOnDateInventoryComponent;
  let fixture: ComponentFixture<AsOnDateInventoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AsOnDateInventoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AsOnDateInventoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
