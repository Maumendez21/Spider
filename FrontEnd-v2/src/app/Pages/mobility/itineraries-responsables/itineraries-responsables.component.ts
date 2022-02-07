import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-itineraries-responsables',
  templateUrl: './itineraries-responsables.component.html',
  styleUrls: ['./itineraries-responsables.component.css']
})
export class ItinerariesResponsablesComponent implements OnInit {

  responsables: any[];
  data: any;
  grupo: string;
  device: string;
  fechaini: string;
  fechafin: string;

  constructor() { }

  ngOnInit(): void {
  }

  get(res){
    this.responsables = res[0].responsables;
    this.grupo = res[0].datos.compania;
    this.device = res[0].datos.device;
    this.fechaini = res[0].datos.fechaIni;
    this.fechafin = res[0].datos.fechaEnd;
  }

  showDetailTravel(device: string, startDate: string, endDate: string): string {
    return `/home/trip/${device}/${startDate}/${endDate}`;
  }

  generarReporte(){
    return `http://spiderfleetapi.azurewebsites.net/api/report/itinerarios/responsible?grupo=${this.grupo}&device=${this.device}&start=${this.fechaini}&end=${this.fechafin}`
  }
}
