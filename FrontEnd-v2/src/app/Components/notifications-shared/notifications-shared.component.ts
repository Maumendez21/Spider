import { Component, Input, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { MapService } from 'src/app/Pages/home/Services/map.service';
declare const google: any;

@Component({
  selector: 'app-notifications-shared',
  templateUrl: './notifications-shared.component.html',
  styleUrls: ['./notifications-shared.component.css']
})
export class NotificationsSharedComponent implements OnInit {

  constructor(
    private mapService: MapService,
  ) { }

  @Input() dtTrigger = new Subject();
  @Input() dtOptions: DataTables.Settings = {};
  @Input() notifications = [];

  ngOnInit(): void {
  }

  public latitud: number = 19.4525976;
  public longitud: number = -99.1182164;
  public zoom: number = 11;
  public map: any;
  public points: any = [];
  public fitBounds: boolean = true;
  public agmFitBounds: boolean = true;
  public bermudaTriangle: any;

  onMapReady(map: any) {
    this.map = map;
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


  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    this.dtTrigger.unsubscribe();
  }

}
