import { Component, OnInit } from '@angular/core';
import { InfoDevice } from '../../home/models/info-device';
import { Maps } from '../../../utils/maps';
import { GeofencesService } from '../services/geofences.service';
import { MapService } from '../../home/Services/map.service';
import { Router } from '@angular/router';
import { Icons } from '../../../utils/iconos';

@Component({
  selector: 'app-monitoring',
  templateUrl: './monitoring.component.html',
  styleUrls: ['./monitoring.component.css']
})
export class MonitoringComponent implements OnInit {

  constructor(
    private mapHelper: Maps,
    private geofencesService: GeofencesService,
    private mapService: MapService,
    private router: Router,
    private helperIcons: Icons,
  ) {
    this.getListGeocercas();
    this.infoDevice = new InfoDevice();
    this.style = mapHelper.style;
  }

  paroMotor: any;

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

  rutaLogo: string;

  // * Icons Devices Map
  icons: any = {
    1: { 'icon': 'assets/images/map/coche_verde.png' },
    2: { 'icon': 'assets/images/map/coche_gris.png' },
    3: { 'icon': 'assets/images/map/coche_amarillo.png' },
    4: { 'icon': 'assets/images/map/coche_naranja.png' },
    5: { 'icon': 'assets/images/map/coche_azul.png' },
    6: { 'icon': 'assets/images/map/coche_rojo.png' },
    7: { 'icon': 'assets/images/map/coche_rojo.png'}
  };

  // * FitBounds
  fitBounds: boolean = false;
  agmFitBounds: boolean = false;


  devicesInterval: any;

  ngOnInit(): void {
  }

  getListGeocercas() {
    this.geofencesService.getListGeocercasMonitoreo()
        .subscribe(data => {
          this.geofences = data;
        })
  }

  openWindow(device: string, estatus: number) {
    this.getInfoDevice(device, estatus);
    document.getElementById("btnInfoMonitoring").click();
  }

  tipo: number =0;


  async getInfoDevice(device: string, estatus: number) {
    await this.mapService.getInfoDevice(device)
      .subscribe(data => {
        this.tipo = data['Itineraries'].TypeDevice;
        this.rutaLogo = this.helperIcons.iconosFrente[data['Itineraries'].TypeDevice][estatus].icon;
        this.infoDevice = data['Itineraries'];
        this.infoDevice['estatus'] = estatus;
      }, error => {
        this.router.navigate(['/login']);
      });
  }


  onMapReady(map) {
    this.map = map;
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
    this.geofencesService.getDevicesGeofenceMonitoreo(this.idGeofence)
        .subscribe(data => {
          this.devices = data;
        });
  }

  public markersFit: any = [];

  drawPolygon() {

    let latlngbounds = new google.maps.LatLngBounds();
    this.markersFit = [];
    this.map.fitBounds(latlngbounds);




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
    this.points[0].map(point => {

      this.markersFit.push(new google.maps.LatLng(point['lat'], point['lng']));
    })
    
    this.bermudaTriangle.setMap(this.map);
    
    for (let i = 0; i < this.markersFit.length; i++) {
      latlngbounds.extend(this.markersFit[i]);
    }

    this.map.fitBounds(latlngbounds);



  }


}
