import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MaterialToleranceComponent } from './material-tolerance.component';

describe('MaterialToleranceComponent', () => {
  let component: MaterialToleranceComponent;
  let fixture: ComponentFixture<MaterialToleranceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MaterialToleranceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MaterialToleranceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
