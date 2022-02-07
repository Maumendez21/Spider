import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NuevoEventoRutaAgendaModalComponent } from './nuevo-evento-ruta-agenda-modal.component';

describe('NuevoEventoRutaAgendaModalComponent', () => {
  let component: NuevoEventoRutaAgendaModalComponent;
  let fixture: ComponentFixture<NuevoEventoRutaAgendaModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NuevoEventoRutaAgendaModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NuevoEventoRutaAgendaModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
