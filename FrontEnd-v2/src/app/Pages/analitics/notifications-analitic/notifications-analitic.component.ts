import { Component, OnInit } from '@angular/core';
import moment from 'moment';
import Swal from 'sweetalert2';
import { MapService } from '../../home/Services/map.service';
import { AnaliticService } from '../services/analitic.service';

@Component({
  selector: 'app-notifications-analitic',
  templateUrl: './notifications-analitic.component.html',
  styleUrls: ['./notifications-analitic.component.css']
})
export class NotificationsAnaliticComponent implements OnInit {

  constructor(
    private mapService: MapService,
    private analiticService: AnaliticService
  ) { }

  // dtTrigger = new Subject();
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

  pageOfItems:Array<any>;

  ngOnInit(): void {
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

  onMapReady(map: any) {
    this.map = map;
  }

  async filtrar(){

    if (this.validateDate()) {

      const data = {
        fechaIni: this.fechaInicio + ' 00:00:00',
        fechaEnd: this.fechaFin  + ' 23:59:00'
      }
      this.cargando = true;

      await this.analiticService.getNotificatonsAnaliticas(data.fechaIni, data.fechaEnd)
      .subscribe(data => {

        this.notifications = data['listNotifications'];
        this.cargando  = false;
      })
    }
    else
    {
      Swal.fire({
        icon: 'error',
        title: 'Fechas Invalidas'
      })
    }
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

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }

  generarReporte(){
    const idu = localStorage.getItem("idu");

    return `http://spiderfleetapi.azurewebsites.net/api/dashboard/activity/day/report/analitica?param=${idu}&fechainicio=${this.fechaInicio}&fechafin=${this.fechaFin}`
  }

  formatPolygonGeofences(id: number) {

    this.notifications[id]['Coordinates'].forEach(x => {
      this.points.push({
        lng: x[0],
        lat: x[1]
      });
    });
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
    this.mapService.setChangeStatusNotification(id)
      .subscribe(data => {
        this.notifications[idCamb].View = 1;
        // this.toastr.success("Notificación actualizada exitosamente");
        });
  }


}
