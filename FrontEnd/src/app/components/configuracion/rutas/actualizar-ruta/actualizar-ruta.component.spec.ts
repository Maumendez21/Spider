import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ActualizarRutaComponent } from './actualizar-ruta.component';

describe('ActualizarRutaComponent', () => {
  let component: ActualizarRutaComponent;
  let fixture: ComponentFixture<ActualizarRutaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ActualizarRutaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ActualizarRutaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
