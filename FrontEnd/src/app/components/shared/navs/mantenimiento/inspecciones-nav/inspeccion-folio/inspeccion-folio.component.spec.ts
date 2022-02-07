import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InspeccionFolioComponent } from './inspeccion-folio.component';

describe('InspeccionFolioComponent', () => {
  let component: InspeccionFolioComponent;
  let fixture: ComponentFixture<InspeccionFolioComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InspeccionFolioComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InspeccionFolioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
