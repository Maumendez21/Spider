import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {



  show: boolean;
  usuario: number;
  devices: any = [];
  subempresas: any = [];

  grupo: string = "";
  unidad: string = "";
  fechaInicio: string;
  fechaFin: string;
  horaInicio: string = "00:00";
  horaFin: string = "23:59";
  itinerarios: string = "1";

  permisos: string;

  production: boolean;

  constructor(private shared: SharedService, private spiderService: SpiderfleetService, private datePipe: DatePipe) {

    if (shared.verifyLoggin()) {
      this.usuario = +localStorage.getItem("idu");
    }

    this.production = environment.production;

    this.showSidebar();
    this.preparePermisos();
    this.idu = localStorage.getItem("idu");
  }

  showSidebar() {

    this.shared.loggedStream$.subscribe(data => {
      this.show = data;
    });

    if (!this.show) {
      this.shared.broadcastLoggedStream((localStorage.getItem("token") ? true : false ));
    }
  }

  initReportes() {
    this.getListDevices();
    this.getSubempresas();

    this.fechaInicio = this.datePipe.transform(new Date(), 'yyyy-MM-dd');
    this.fechaFin = this.datePipe.transform(new Date(), 'yyyy-MM-dd');
  }

  preparePermisos() {
    this.shared.permisosStream$.subscribe(response => {
      this.permisos = response;

    });

    if (!this.permisos) {
      this.permisos = localStorage.getItem('permits');
    }
  }

  generarReporte() {
    if (this.grupo != "" && this.grupo != null) {
      const idu = localStorage.getItem("idu");
      const company = localStorage.getItem("company");
      return `http://spiderfleetapi.azurewebsites.net/api/reporte/viajes/mongo/zip?empresa=${company}&param=${idu}&type=${this.itinerarios}&grupo=${this.grupo}&device=${this.unidad}&fechainicio=${this.fechaInicio + " " + this.horaInicio}&fechafin=${this.fechaFin + " " + this.horaFin}`;
      
      // http://spiderfleetapi.azurewebsites.net/api/reporte/viajes/mongo/zip?empresa=0¶m=38&type=1&grupo=/38/2/&device=213WP2018002059&fechainicio=2021-10-07 00:00&fechafin=2021-10-07 23:59
    
    
    }
  }
  idu: string = "";

  checkIdu(): boolean{
    switch (this.idu) {
      case '73':
        return false
      case '80':
        return false
      case '81':
        return false
      case '81-10':
        return false
      default:
        return true
    }
  }

  getSubempresas() {
    this.spiderService.getSubempresas()
      .subscribe(data => {
        this.subempresas = data;
      });
  }

  ngOnInit(): void {
  }

  updateSubempresa() {
    this.unidad = "";
  }

  toggleListgroup(id: string) {

    const element = document.getElementById(id);

    if (element.style.display == "block") {
      element.style.display = "none";
    } else {
      element.style.display = "block";
    }
  }

  getListDevices() {
    this.spiderService.getListDevicesConfiguration()
      .subscribe(data => {
        this.devices = data;
      });
  }

  signOut() {
    this.shared.signOut();
  }

}
