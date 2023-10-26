import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WarrantyClaimComponent } from './warranty-claim.component';

describe('WarrantyClaimComponent', () => {
  let component: WarrantyClaimComponent;
  let fixture: ComponentFixture<WarrantyClaimComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WarrantyClaimComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WarrantyClaimComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
