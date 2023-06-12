import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DiMatIframeDialogComponent } from './di-mat-iframe-dialog.component';

describe('DiMatIframeDialogComponent', () => {
  let component: DiMatIframeDialogComponent;
  let fixture: ComponentFixture<DiMatIframeDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DiMatIframeDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DiMatIframeDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
