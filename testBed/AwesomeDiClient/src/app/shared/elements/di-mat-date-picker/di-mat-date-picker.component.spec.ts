import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DiMatDatePickerComponent } from './di-mat-date-picker.component';

describe('DiMatDatePickerComponent', () => {
  let component: DiMatDatePickerComponent;
  let fixture: ComponentFixture<DiMatDatePickerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DiMatDatePickerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DiMatDatePickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
