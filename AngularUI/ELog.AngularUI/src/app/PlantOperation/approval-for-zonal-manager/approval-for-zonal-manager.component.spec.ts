import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApprovalForZonalManagerComponent } from './approval-for-zonal-manager.component';

describe('ApprovalForZonalManagerComponent', () => {
  let component: ApprovalForZonalManagerComponent;
  let fixture: ComponentFixture<ApprovalForZonalManagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApprovalForZonalManagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApprovalForZonalManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
