import { Component, OnInit } from '@angular/core';
import { Maps } from '../../../utils/maps';
declare const google: any;

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent implements OnInit {



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

  constructor(
    private mapHelper: Maps
  ) {
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

    console.log(this.points);
    

    this.bermudaTriangle.setMap(this.map);
    for (let i = 0; i < this.markersFit.length; i++) {
      latlngbounds.extend(this.markersFit[i]);
    }

    this.map.fitBounds(latlngbounds);
  }

}
