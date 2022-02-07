import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EliminarAsignacionPuntosInteresComponent } from './eliminar-asignacion-puntos-interes.component';

describe('EliminarAsignacionPuntosInteresComponent', () => {
  let component: EliminarAsignacionPuntosInteresComponent;
  let fixture: ComponentFixture<EliminarAsignacionPuntosInteresComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EliminarAsignacionPuntosInteresComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EliminarAsignacionPuntosInteresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
