import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FriendsAddComponent } from './friends-add.component';

describe('FriendsAddComponent', () => {
  let component: FriendsAddComponent;
  let fixture: ComponentFixture<FriendsAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FriendsAddComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FriendsAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
