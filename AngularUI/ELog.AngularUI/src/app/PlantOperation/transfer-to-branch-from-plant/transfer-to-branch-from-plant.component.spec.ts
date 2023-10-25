import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TransferToBranchFromPlantComponent } from './transfer-to-branch-from-plant.component';

describe('TransferToBranchFromPlantComponent', () => {
  let component: TransferToBranchFromPlantComponent;
  let fixture: ComponentFixture<TransferToBranchFromPlantComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TransferToBranchFromPlantComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TransferToBranchFromPlantComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
