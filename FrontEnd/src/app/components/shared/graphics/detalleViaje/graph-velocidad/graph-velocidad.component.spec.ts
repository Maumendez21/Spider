import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GraphVelocidadComponent } from './graph-velocidad.component';

describe('GraphVelocidadComponent', () => {
  let component: GraphVelocidadComponent;
  let fixture: ComponentFixture<GraphVelocidadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GraphVelocidadComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GraphVelocidadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
