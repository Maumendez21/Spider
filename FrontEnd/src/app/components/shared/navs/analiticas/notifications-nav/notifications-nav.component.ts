import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import moment from 'moment';

declare const google: any;

@Component({
  selector: 'app-notifications-nav',
  templateUrl: './notifications-nav.component.html',
  styleUrls: ['./notifications-nav.component.css']
})
export class NotificationsNavComponent  {

  dtTrigger = new Subject();
  dtOptions: DataTables.Settings = {};
  notifications: any[] = [];

  fechaInicio: string = "";
  fechaFin: string = "";
  cargando: boolean = false;

  arre: any[] = [];

  latitud: number = 19.4525976;
  longitud: number = -99.1182164;
  zoom: number = 11;
  map: any;

  points: any = [];

  fitBounds: boolean = true;
  agmFitBounds: boolean = true;

  bermudaTriangle: any;

  constructor(private router: Router, private shared: SharedService, private spiderService: SpiderfleetService, private toastr: ToastrService) {
    this.limpiarFiltrosMapa();
    if (shared.verifyLoggin()) {

      // this.filtrar();

    } else {
      this.router.navigate(['/login']);
    }
  }

  limpiarFiltrosMapa() {
    this.shared.limpiarFiltros();
    if (!document.getElementById("sidebarRight").classList.toggle("active")) {
      document.getElementById("sidebarRight").classList.toggle("active")
    }
  }

  generarReporte(){
    const idu = localStorage.getItem("idu");

    return `http://spiderfleetapi.azurewebsites.net/api/dashboard/activity/day/report/analitica?param=${idu}&fechainicio=${this.fechaInicio}&fechafin=${this.fechaFin}`
  }

  fechaInicioInvalid:  boolean = false;
  fechaInicioTemp: boolean = false;
  fechaFinTemp: boolean = false;

  validateDate(): boolean{
    this.fechaInicioTemp =  moment(this.fechaInicio).isAfter(moment().format('YYYY-MM-DD'));
    this.fechaFinTemp =  moment(this.fechaFin).isAfter(moment().format('YYYY-MM-DD'));
    this.fechaInicioInvalid = moment(this.fechaInicio).isAfter(moment(this.fechaFin));
    return ((!this.fechaInicioTemp && !this.fechaFinTemp) && (!this.fechaInicioInvalid)) ? true : false
  }


  async filtrar(){

    if (this.validateDate()) {

      const data = {
        fechaIni: this.fechaInicio + ' 00:00:00',
        fechaEnd: this.fechaFin  + ' 23:59:00'
      }
      this.cargando = true;

      await this.spiderService.getNotificatonsAnaliticas(data.fechaIni, data.fechaEnd)
      .subscribe(data => {

        this.notifications = data['listNotifications'];
        this.cargando  = false;
      })
    }
    else
    {
      this.toastr.warning("Fechas invalidas.");
    }






  }

  pageOfItems:Array<any>;

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }

  onMapReady(map: any) {
    this.map = map;
  }

  drawPolygon(id: number) {

    if (this.points.length > 0) {
      this.bermudaTriangle.setMap(null);
      this.points = [];
    }

    this.formatPolygonGeofences(id);

    this.bermudaTriangle = new google.maps.Polygon({
      paths: this.points,
      strokeColor: "#FF0000",
      strokeOpacity: 0.8,
      strokeWeight: 2,
      fillColor: "#FF0000",
      fillOpacity: 0.35,
      editable: false,
      draggable: false,
    });

    this.bermudaTriangle.setMap(this.map);
  }

  formatPolygonGeofences(id: number) {

    this.notifications[id]['Coordinates'].forEach(x => {
      this.points.push({
        lng: x[0],
        lat: x[1]
      });
    });
  }


  ngOnInit(): void {
  }

  showMap(id: string, latitud: number, longitud: number, idCambiar: number) {
    this.changeEstatusNotification(id, idCambiar);
    this.latitud = latitud;
    this.longitud = longitud;
    if (this.notifications[idCambiar]['Coordinates'].length > 0) {
      this.drawPolygon(idCambiar);
    }

    console.log(id, latitud, longitud, idCambiar);

  }


  changeEstatusNotification(id: string, idCamb: number) {
    this.spiderService.setChangeStatusNotification(id)
      .subscribe(data => {
        this.notifications[idCamb].View = 1;
        this.toastr.success("Notificación actualizada exitosamente");
        });
  }


}
