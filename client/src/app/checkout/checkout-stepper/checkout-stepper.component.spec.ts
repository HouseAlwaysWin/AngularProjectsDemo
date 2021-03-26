import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CheckoutStepper } from './checkout-stepper.component';


describe('CheckoutStepperComponent', () => {
  let component: CheckoutStepper;
  let fixture: ComponentFixture<CheckoutStepper>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CheckoutStepper]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CheckoutStepper);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
