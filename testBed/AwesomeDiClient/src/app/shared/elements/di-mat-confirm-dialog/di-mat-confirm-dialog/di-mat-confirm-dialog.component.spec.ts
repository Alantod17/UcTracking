import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DiMatConfirmDialogComponent } from './di-mat-confirm-dialog.component';

describe('DiMatConfirmDialogComponent', () => {
  let component: DiMatConfirmDialogComponent;
  let fixture: ComponentFixture<DiMatConfirmDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DiMatConfirmDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DiMatConfirmDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
