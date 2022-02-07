import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NuevoPuntoInteresComponent } from './nuevo-punto-interes.component';

describe('NuevoPuntoInteresComponent', () => {
  let component: NuevoPuntoInteresComponent;
  let fixture: ComponentFixture<NuevoPuntoInteresComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NuevoPuntoInteresComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NuevoPuntoInteresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
