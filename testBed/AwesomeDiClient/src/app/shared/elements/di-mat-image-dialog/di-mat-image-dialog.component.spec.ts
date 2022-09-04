import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DiMatImageDialogComponent } from './di-mat-image-dialog.component';

describe('DiMatImageDialogComponent', () => {
  let component: DiMatImageDialogComponent;
  let fixture: ComponentFixture<DiMatImageDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DiMatImageDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DiMatImageDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
