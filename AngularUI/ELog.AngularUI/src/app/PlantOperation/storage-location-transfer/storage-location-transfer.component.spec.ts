import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StorageLocationTransferComponent } from './storage-location-transfer.component';

describe('StorageLocationTransferComponent', () => {
  let component: StorageLocationTransferComponent;
  let fixture: ComponentFixture<StorageLocationTransferComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StorageLocationTransferComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StorageLocationTransferComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
