import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RevalidationProcessBranchComponent } from './revalidation-process-branch.component';

describe('RevalidationProcessBranchComponent', () => {
  let component: RevalidationProcessBranchComponent;
  let fixture: ComponentFixture<RevalidationProcessBranchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RevalidationProcessBranchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RevalidationProcessBranchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
