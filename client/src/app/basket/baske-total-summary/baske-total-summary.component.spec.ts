import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BaskeTotalSummaryComponent } from './baske-total-summary.component';

describe('BaskeTotalSummaryComponent', () => {
  let component: BaskeTotalSummaryComponent;
  let fixture: ComponentFixture<BaskeTotalSummaryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BaskeTotalSummaryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BaskeTotalSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
