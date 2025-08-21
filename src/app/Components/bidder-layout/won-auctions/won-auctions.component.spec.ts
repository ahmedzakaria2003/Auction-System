import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WonAuctionsComponent } from './won-auctions.component';

describe('WonAuctionsComponent', () => {
  let component: WonAuctionsComponent;
  let fixture: ComponentFixture<WonAuctionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WonAuctionsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WonAuctionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
