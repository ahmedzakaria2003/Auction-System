import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerStatisticsComponent } from './statistics.component';

describe('StatisticsComponent', () => {
  let component: SellerStatisticsComponent;
  let fixture: ComponentFixture<SellerStatisticsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SellerStatisticsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SellerStatisticsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
