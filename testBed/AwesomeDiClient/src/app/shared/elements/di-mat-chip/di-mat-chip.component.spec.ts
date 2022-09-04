import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DiMatChipComponent } from './di-mat-chip.component';

describe('DiMatChipComponent', () => {
  let component: DiMatChipComponent;
  let fixture: ComponentFixture<DiMatChipComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DiMatChipComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DiMatChipComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
