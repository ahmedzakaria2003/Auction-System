import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerReportsComponent } from './reports.component';

describe('ReportsComponent', () => {
  let component: SellerReportsComponent;
  let fixture: ComponentFixture<SellerReportsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SellerReportsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SellerReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
