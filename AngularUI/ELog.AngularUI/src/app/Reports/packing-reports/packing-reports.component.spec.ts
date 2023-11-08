import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PackingReportsComponent } from './packing-reports.component';

describe('PackingReportsComponent', () => {
  let component: PackingReportsComponent;
  let fixture: ComponentFixture<PackingReportsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PackingReportsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PackingReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
