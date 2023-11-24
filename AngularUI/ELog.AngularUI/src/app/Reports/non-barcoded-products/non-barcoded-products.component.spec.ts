import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NonBarcodedProductsComponent } from './non-barcoded-products.component';

describe('NonBarcodedProductsComponent', () => {
  let component: NonBarcodedProductsComponent;
  let fixture: ComponentFixture<NonBarcodedProductsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NonBarcodedProductsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NonBarcodedProductsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
