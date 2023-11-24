import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsolidatedNonBarcodedProductsComponent } from './consolidated-non-barcoded-products.component';

describe('ConsolidatedNonBarcodedProductsComponent', () => {
  let component: ConsolidatedNonBarcodedProductsComponent;
  let fixture: ComponentFixture<ConsolidatedNonBarcodedProductsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConsolidatedNonBarcodedProductsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConsolidatedNonBarcodedProductsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
