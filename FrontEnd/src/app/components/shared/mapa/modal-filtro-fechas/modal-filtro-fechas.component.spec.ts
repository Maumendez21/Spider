import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalFiltroFechasComponent } from './modal-filtro-fechas.component';

describe('ModalFiltroFechasComponent', () => {
  let component: ModalFiltroFechasComponent;
  let fixture: ComponentFixture<ModalFiltroFechasComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModalFiltroFechasComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalFiltroFechasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
