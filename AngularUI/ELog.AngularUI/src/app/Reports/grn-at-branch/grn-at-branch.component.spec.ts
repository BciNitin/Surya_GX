import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GrnAtBranchComponent } from './grn-at-branch.component';

describe('GrnAtBranchComponent', () => {
  let component: GrnAtBranchComponent;
  let fixture: ComponentFixture<GrnAtBranchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GrnAtBranchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GrnAtBranchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
