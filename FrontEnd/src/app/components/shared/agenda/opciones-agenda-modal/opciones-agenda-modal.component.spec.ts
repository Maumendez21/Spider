import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OpcionesAgendaModalComponent } from './opciones-agenda-modal.component';

describe('OpcionesAgendaModalComponent', () => {
  let component: OpcionesAgendaModalComponent;
  let fixture: ComponentFixture<OpcionesAgendaModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OpcionesAgendaModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OpcionesAgendaModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
