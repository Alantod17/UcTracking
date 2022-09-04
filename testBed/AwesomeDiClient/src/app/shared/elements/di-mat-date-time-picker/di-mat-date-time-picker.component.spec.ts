import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DiMatDateTimePickerComponent } from './di-mat-date-time-picker.component';

describe('DiMatDateTimePickerComponent', () => {
  let component: DiMatDateTimePickerComponent;
  let fixture: ComponentFixture<DiMatDateTimePickerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DiMatDateTimePickerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DiMatDateTimePickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
