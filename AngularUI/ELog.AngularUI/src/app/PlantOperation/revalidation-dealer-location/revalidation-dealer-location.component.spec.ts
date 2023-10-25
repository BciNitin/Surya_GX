import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RevalidationDealerLocationComponent } from './revalidation-dealer-location.component';

describe('RevalidationDealerLocationComponent', () => {
  let component: RevalidationDealerLocationComponent;
  let fixture: ComponentFixture<RevalidationDealerLocationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RevalidationDealerLocationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RevalidationDealerLocationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
