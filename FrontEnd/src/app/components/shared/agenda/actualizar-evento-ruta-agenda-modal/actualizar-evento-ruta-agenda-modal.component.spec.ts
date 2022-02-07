import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ActualizarEventoRutaAgendaModalComponent } from './actualizar-evento-ruta-agenda-modal.component';

describe('ActualizarEventoRutaAgendaModalComponent', () => {
  let component: ActualizarEventoRutaAgendaModalComponent;
  let fixture: ComponentFixture<ActualizarEventoRutaAgendaModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ActualizarEventoRutaAgendaModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ActualizarEventoRutaAgendaModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
