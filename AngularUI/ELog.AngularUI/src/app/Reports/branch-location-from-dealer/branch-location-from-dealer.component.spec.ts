import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BranchLocationFromDealerComponent } from './branch-location-from-dealer.component';

describe('BranchLocationFromDealerComponent', () => {
  let component: BranchLocationFromDealerComponent;
  let fixture: ComponentFixture<BranchLocationFromDealerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BranchLocationFromDealerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BranchLocationFromDealerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
