import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SerialbarcodegenerationComponent } from './serialbarcodegeneration.component';

describe('SerialbarcodegenerationComponent', () => {
  let component: SerialbarcodegenerationComponent;
  let fixture: ComponentFixture<SerialbarcodegenerationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SerialbarcodegenerationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SerialbarcodegenerationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
