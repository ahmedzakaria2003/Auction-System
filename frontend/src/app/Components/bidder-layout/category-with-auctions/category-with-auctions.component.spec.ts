import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryWithAuctionsComponent } from './category-with-auctions.component';

describe('CategoryWithAuctionsComponent', () => {
  let component: CategoryWithAuctionsComponent;
  let fixture: ComponentFixture<CategoryWithAuctionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategoryWithAuctionsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryWithAuctionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
