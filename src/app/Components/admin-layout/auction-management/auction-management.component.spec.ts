import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuctionManagementComponent } from './auction-management.component';

describe('AuctionManagementComponent', () => {
  let component: AuctionManagementComponent;
  let fixture: ComponentFixture<AuctionManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuctionManagementComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AuctionManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
