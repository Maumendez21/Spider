import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
declare const google: any;

@Component({
  selector: 'app-notifications-shared',
  templateUrl: './notifications-shared.component.html',
  styleUrls: ['./notifications-shared.component.css']
})
export class NotificationsSharedComponent implements OnInit, OnDestroy {

  @Input() dtTrigger = new Subject();
  @Input() dtOptions: DataTables.Settings = {};
  @Input() notifications = [];

  arre: any[] = [];

  latitud: number = 19.4525976;
  longitud: number = -99.1182164;
  zoom: number = 11;
  map: any;

  points: any = [];

  fitBounds: boolean = true;
  agmFitBounds: boolean = true;

  bermudaTriangle: any;
  constructor(private spiderService: SpiderfleetService,  private toastr: ToastrService) {
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

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
  }

}
