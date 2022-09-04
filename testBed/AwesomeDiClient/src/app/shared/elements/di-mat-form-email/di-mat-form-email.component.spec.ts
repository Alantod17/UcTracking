import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DiMatFormEmailComponent } from './di-mat-form-email.component';

describe('DiMatFormEmailComponent', () => {
  let component: DiMatFormEmailComponent;
  let fixture: ComponentFixture<DiMatFormEmailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [DiMatFormEmailComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DiMatFormEmailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
