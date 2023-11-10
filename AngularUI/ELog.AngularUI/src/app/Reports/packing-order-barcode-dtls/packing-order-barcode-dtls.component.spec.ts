import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PackingOrderBarcodeDtlsComponent } from './packing-order-barcode-dtls.component';

describe('PackingOrderBarcodeDtlsComponent', () => {
  let component: PackingOrderBarcodeDtlsComponent;
  let fixture: ComponentFixture<PackingOrderBarcodeDtlsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PackingOrderBarcodeDtlsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PackingOrderBarcodeDtlsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
