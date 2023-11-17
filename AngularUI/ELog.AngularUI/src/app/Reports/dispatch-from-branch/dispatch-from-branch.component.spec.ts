import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DispatchFromBranchComponent } from './dispatch-from-branch.component';

describe('DispatchFromBranchComponent', () => {
  let component: DispatchFromBranchComponent;
  let fixture: ComponentFixture<DispatchFromBranchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DispatchFromBranchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DispatchFromBranchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
