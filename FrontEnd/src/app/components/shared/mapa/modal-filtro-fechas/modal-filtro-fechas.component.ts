import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-modal-filtro-fechas',
  templateUrl: './modal-filtro-fechas.component.html',
  styleUrls: ['./modal-filtro-fechas.component.css']
})
export class ModalFiltroFechasComponent implements OnInit {

  @Input() prepareDates: any;

  fechaInicio: string;
  fechaFin: string;

  constructor() { }

  ngOnInit(): void {
  }

}
