import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from 'src/app/services/shared.service';

@Component({
  selector: 'app-responsables-nav',
  templateUrl: './responsables-nav.component.html',
  styleUrls: ['./responsables-nav.component.css']
})
export class ResponsablesNavComponent  {

  responsables: any[];
  data: any;
  grupo: string;
  device: string;
  fechaini: string;
  fechafin: string;


  constructor(private router: Router, private shared: SharedService) {
    this.limpiarFiltrosMapa();
    if (shared.verifyLoggin()) {

    } else {
      this.router.navigate(['/login']);
    }
  }

  get(res){
    this.responsables = res[0].responsables;
    this.grupo = res[0].datos.compania;
    this.device = res[0].datos.device;
    this.fechaini = res[0].datos.fechaIni;
    this.fechafin = res[0].datos.fechaEnd;
  }

  limpiarFiltrosMapa() {
    this.shared.limpiarFiltros();
    if (!document.getElementById("sidebarRight").classList.toggle("active")) {
      document.getElementById("sidebarRight").classList.toggle("active")
    }
  }

  showDetailTravel(device: string, startDate: string, endDate: string): string {
    return `/trip/${device}/${startDate}/${endDate}`;
  }

  generarReporte(){
    return `http://spiderfleetapi.azurewebsites.net/api/report/itinerarios/responsible?grupo=${this.grupo}&device=${this.device}&start=${this.fechaini}&end=${this.fechafin}`
  }









}
