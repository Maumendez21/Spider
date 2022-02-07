import { Component, OnInit } from '@angular/core';
import { Maps } from 'src/app/helpers/maps';
declare const google: any;
@Component({
  selector: 'app-geocerca-historico-nav',
  templateUrl: './geocerca-historico-nav.component.html',
  styleUrls: ['./geocerca-historico-nav.component.css']
})
export class GeocercaHistoricoNavComponent implements OnInit {


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

  constructor(private mapHelper: Maps) {
    this.style = mapHelper.style;
  }

  ngOnInit(): void {
  }

  onMapReady(map) {
    this.map = map;
  }

  getDatos(data: any){
    if ( data[0].geocerca != "") {
      this.devices = data[0].devices;
      if (this.points.length > 0) {
        this.bermudaTriangle.setMap(null);
      }
      this.points = [data[0].geocercas[data[0].geocerca]["Polygon"]['Coordinates']];
      this.drawPolygon();
      this.fitBounds = true;
      this.agmFitBounds = true;
    }

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


}
