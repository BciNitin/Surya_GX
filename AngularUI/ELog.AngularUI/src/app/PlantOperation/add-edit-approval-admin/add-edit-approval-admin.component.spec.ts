import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEditApprovalAdminComponent } from './add-edit-approval-admin.component';

describe('AddEditApprovalAdminComponent', () => {
  let component: AddEditApprovalAdminComponent;
  let fixture: ComponentFixture<AddEditApprovalAdminComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddEditApprovalAdminComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddEditApprovalAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
