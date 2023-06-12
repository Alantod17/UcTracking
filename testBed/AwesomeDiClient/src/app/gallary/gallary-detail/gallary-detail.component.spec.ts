import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GallaryDetailComponent } from './gallary-detail.component';

describe('GallaryDetailComponent', () => {
  let component: GallaryDetailComponent;
  let fixture: ComponentFixture<GallaryDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GallaryDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GallaryDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
