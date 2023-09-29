import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManualPackingComponent } from './manual-packing.component';

describe('ManualPackingComponent', () => {
  let component: ManualPackingComponent;
  let fixture: ComponentFixture<ManualPackingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManualPackingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManualPackingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
