import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEditZonalComponent } from './add-edit-zonal.component';

describe('AddEditZonalComponent', () => {
  let component: AddEditZonalComponent;
  let fixture: ComponentFixture<AddEditZonalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddEditZonalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddEditZonalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
