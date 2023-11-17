import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlantToWarehouseComponent } from './plant-to-warehouse.component';

describe('PlantToWarehouseComponent', () => {
  let component: PlantToWarehouseComponent;
  let fixture: ComponentFixture<PlantToWarehouseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlantToWarehouseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlantToWarehouseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
