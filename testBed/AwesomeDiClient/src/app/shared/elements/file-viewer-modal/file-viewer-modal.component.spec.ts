import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FileViewerModalComponent } from './file-viewer-modal.component';

describe('FileViewerModalComponent', () => {
  let component: FileViewerModalComponent;
  let fixture: ComponentFixture<FileViewerModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FileViewerModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FileViewerModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
