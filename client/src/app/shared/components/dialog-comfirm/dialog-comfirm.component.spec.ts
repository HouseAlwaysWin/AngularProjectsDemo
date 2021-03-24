import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogComfirm } from './dialog-comfirm.component';

describe('DialogComfirmComponent', () => {
  let component: DialogComfirm;
  let fixture: ComponentFixture<DialogComfirm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DialogComfirm]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogComfirm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
