import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogMessage } from './dialog-message.component';

describe('DialogMessageComponent', () => {
  let component: DialogMessage;
  let fixture: ComponentFixture<DialogMessage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DialogMessage]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogMessage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
