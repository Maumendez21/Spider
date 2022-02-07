import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AsignacionPuntosInteresComponent } from './asignacion-puntos-interes.component';

describe('AsignacionPuntosInteresComponent', () => {
  let component: AsignacionPuntosInteresComponent;
  let fixture: ComponentFixture<AsignacionPuntosInteresComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AsignacionPuntosInteresComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AsignacionPuntosInteresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
