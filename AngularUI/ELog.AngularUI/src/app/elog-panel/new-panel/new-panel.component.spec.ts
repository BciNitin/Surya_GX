import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewPanelComponent } from './new-panel.component';

describe('NewPanelComponent', () => {
  let component: NewPanelComponent;
  let fixture: ComponentFixture<NewPanelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewPanelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
  
  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
