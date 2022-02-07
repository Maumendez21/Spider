import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ActualizarPuntoInteresComponent } from './actualizar-punto-interes.component';

describe('ActualizarPuntoInteresComponent', () => {
  let component: ActualizarPuntoInteresComponent;
  let fixture: ComponentFixture<ActualizarPuntoInteresComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ActualizarPuntoInteresComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ActualizarPuntoInteresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
