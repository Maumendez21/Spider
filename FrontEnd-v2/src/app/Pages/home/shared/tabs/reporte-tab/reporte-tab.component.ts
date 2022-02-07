import { DatePipe } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-reporte-tab',
  templateUrl: './reporte-tab.component.html',
  styleUrls: ['./reporte-tab.component.css']
})
export class ReporteTabComponent implements OnInit {

  constructor(private datePipe: DatePipe) {
    this.fechaInicio = this.datePipe.transform(new Date(), 'yyyy-MM-dd');
    this.fechaFin = this.datePipe.transform(new Date(), 'yyyy-MM-dd');
  }
  @Input() device: any;
  @Input() empresa: any;

  public grupo: string = "";
  public unidad: string = "";
  public fechaInicio: string;
  public fechaFin: string;
  public horaInicio: string = "00:00";
  public horaFin: string = "23:59";
  public itinerarios: string = "1";


  generarReporte() {
    const idu = localStorage.getItem("idu");
    const company = localStorage.getItem("company");
    return `http://spiderfleetapi.azurewebsites.net/api/reporte/viajes/mongo/zip?empresa=${company}&param=${idu}&type=${this.itinerarios}&grupo=${this.grupo}&device=${this.device}&fechainicio=${this.fechaInicio + " " + this.horaInicio}&fechafin=${this.fechaFin + " " + this.horaFin}`;

}

  ngOnInit(): void {
  }

}
