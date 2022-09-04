import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DiBooleanToggleComponent } from './di-boolean-toggle.component';

describe('DiBooleanToggleComponent', () => {
  let component: DiBooleanToggleComponent;
  let fixture: ComponentFixture<DiBooleanToggleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DiBooleanToggleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DiBooleanToggleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
