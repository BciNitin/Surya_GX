import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DispatchFromWarehouseComponent } from './dispatch-from-warehouse.component';

describe('DispatchFromWarehouseComponent', () => {
  let component: DispatchFromWarehouseComponent;
  let fixture: ComponentFixture<DispatchFromWarehouseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DispatchFromWarehouseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DispatchFromWarehouseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
