import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-opciones-agenda-modal',
  templateUrl: './opciones-agenda-modal.component.html',
  styleUrls: ['./opciones-agenda-modal.component.css']
})
export class OpcionesAgendaModalComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  modalResponsable() {
    document.getElementById("clickNuevoEventoModal").click();
  }

  modalRuta() {
    document.getElementById("clickNuevoEventoRutaModal").click();
  }

}
