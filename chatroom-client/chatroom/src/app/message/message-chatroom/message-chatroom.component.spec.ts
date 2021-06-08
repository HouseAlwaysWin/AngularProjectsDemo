import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MessageChatroomComponent } from './message-chatroom.component';

describe('MessageChatroomComponent', () => {
  let component: MessageChatroomComponent;
  let fixture: ComponentFixture<MessageChatroomComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MessageChatroomComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MessageChatroomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
