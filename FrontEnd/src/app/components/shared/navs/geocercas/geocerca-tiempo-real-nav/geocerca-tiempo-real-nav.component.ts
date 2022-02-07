import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Maps } from 'src/app/helpers/maps';
import { InfoDevice } from 'src/app/models/info-device/info-device';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
declare const google: any;
@Component({
  selector: 'app-geocerca-tiempo-real-nav',
  templateUrl: './geocerca-tiempo-real-nav.component.html',
  styleUrls: ['./geocerca-tiempo-real-nav.component.css']
})
export class GeocercaTiempoRealNavComponent implements OnInit {

  geofences: any;
  geofence: string = "";
  idGeofence: string = "";

  map: any;
  center: any = {
    lat: 19.4525976,
    lng: -99.1182164
  };
  points: any = [];
  devices: any = [];
  style: any;

  infoDevice: InfoDevice;

  bermudaTriangle: any;

  // * Icons Devices Map
  icons: any = {
    1: { 'icon': 'assets/images/coche_verde.png' },
    2: { 'icon': 'assets/images/coche_gris.png' },
    3: { 'icon': 'assets/images/coche_amarillo.png' },
    4: { 'icon': 'assets/images/coche_naranja.png' },
    5: { 'icon': 'assets/images/coche_azul.png' },
    6: { 'icon': 'assets/images/coche_rojo.png' },
    7: { 'icon': 'assets/images/coche_rojo.png'}
  };

  // * FitBounds
  fitBounds: boolean = false;
  agmFitBounds: boolean = false;

  devicesInterval: any;

  constructor(private spiderService: SpiderfleetService, private shared: SharedService, private router: Router, private mapHelper: Maps) {
    if (shared.verifyLoggin()) {
      this.limpiarFiltrosMapa();
      this.getListGeocercas();
      this.infoDevice = new InfoDevice();
      this.style = mapHelper.style;
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

  ngOnInit(): void {
  }

  onMapReady(map) {
    this.map = map;
  }

  drawPolygon() {
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

  getListGeocercas() {
    this.spiderService.getListGeocercasMonitoreo()
        .subscribe(data => {
          this.geofences = data;
        })
  }

  prepareDrawGeofence() {
    if (this.geofence != "") {

      if (this.points.length > 0) {
        this.bermudaTriangle.setMap(null);
      }

      this.points = [this.geofences[this.geofence]["Polygon"]['Coordinates']];
      this.idGeofence = this.geofences[this.geofence]["Id"];

      this.getDevices();

      setInterval(() => {
        this.getDevices();
      }, 30000);

      this.drawPolygon();

      this.fitBounds = true;
      this.agmFitBounds = true;
    }
  }

  getDevices() {
    this.spiderService.getDevicesGeofenceMonitoreo(this.idGeofence)
        .subscribe(data => {
          this.devices = data;
        });
  }

  openWindow(device: string, estatus: number) {
    this.getInfoDevice(device, estatus);
    document.getElementById("btnModalInfoDevice").click();
  }

  async getInfoDevice(device: string, estatus: number) {
    await this.spiderService.getInfoDevice(device)
      .subscribe(data => {
        this.infoDevice = data['Itineraries'];
        this.infoDevice['estatus'] = estatus;
      }, error => {
        this.shared.broadcastLoggedStream(false);
        this.shared.clearSharedSession();
        this.router.navigate(['/login']);
      });
  }

  getEstatusName(estatus: number) {
    switch(estatus) {
      case 1:
        return "Activo";
      case 2:
        return "Inactivo";
      case 3:
        return "Inactivo/Warning";
      case 4:
        return "Falla/Error";
      case 5:
        return "Activo sin movimiento";
      case 6:
        return "Paro";
      case 7:
        return "Pánico";
    }
  }

}
