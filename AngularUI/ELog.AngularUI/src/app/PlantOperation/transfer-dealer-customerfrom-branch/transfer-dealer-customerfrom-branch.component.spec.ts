import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TransferDealerCustomerfromBranchComponent } from './transfer-dealer-customerfrom-branch.component';

describe('TransferDealerCustomerfromBranchComponent', () => {
  let component: TransferDealerCustomerfromBranchComponent;
  let fixture: ComponentFixture<TransferDealerCustomerfromBranchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TransferDealerCustomerfromBranchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TransferDealerCustomerfromBranchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
