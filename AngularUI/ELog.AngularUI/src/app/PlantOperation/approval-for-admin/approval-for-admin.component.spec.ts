import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApprovalForAdminComponent } from './approval-for-admin.component';

describe('ApprovalForAdminComponent', () => {
  let component: ApprovalForAdminComponent;
  let fixture: ComponentFixture<ApprovalForAdminComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApprovalForAdminComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApprovalForAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
