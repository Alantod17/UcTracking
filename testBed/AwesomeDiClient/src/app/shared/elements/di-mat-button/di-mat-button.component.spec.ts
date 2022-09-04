import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DiMatButtonComponent } from './di-mat-button.component';

describe('DiMatButtonComponent', () => {
  let component: DiMatButtonComponent;
  let fixture: ComponentFixture<DiMatButtonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DiMatButtonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DiMatButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
