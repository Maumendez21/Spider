import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-consult-points-interest',
  templateUrl: './consult-points-interest.component.html',
  styleUrls: ['./consult-points-interest.component.css']
})
export class ConsultPointsInterestComponent implements OnInit {

  constructor() { }

  center: any = {
    lat: 19.4525976,
    lng: -99.1182164
  };

  drawingManager: any;

  markersMap: any;
  geofencesMap: any;


  latitud: number = 19.4525976;
  longitud: number = -99.1182164;

  // Punto de interés
  InterestPoint: any;
  latPoint: number = 0;
  lonPoint: number = 0;
  nombre: string;
  coordinates: any;

  zoom: number = 22;
  map: any;

  fitBounds: boolean = true;
  agmFitBounds: boolean = true;

  puntos: any = [];

  grupo: string;
  device: string;
  fechaini: string;
  fechafin: string;
  punto: string;


  ngOnInit(): void {
  }

  
  onMapReady(map) {
    this.map = map;
  }

  getPuntos(data: any[]){
    this.puntos = data[0].puntos;
    this.InterestPoint = data[0].puntos['PointInterest'];

    this.grupo = data[0].datos.compania;
    this.device = data[0].datos.device;
    this.fechaini = data[0].datos.fechaIni;
    this.fechafin = data[0].datos.fechaEnd;
    this.punto = data[0].datos.punto;
  }

  showMap(latitud: number, longitud: number){
    this.latitud = latitud;
    this.longitud = longitud;

    this.nombre       =  this.InterestPoint.Name;
    this.latPoint     =  this.InterestPoint.Latitude;
    this.lonPoint     =  this.InterestPoint.Longitude;
    this.drawMarkersInMap();
    // this.nombre = alarma;
  }
  generarReporte(){
    return `http://spiderfleetapi.azurewebsites.net/api/report/point/interest/analysis?mongo=${this.punto}&grupo=${this.grupo}&device=${this.device}&start=${this.fechaini}&end=${this.fechafin}`
  }

  drawMarkersInMap() {

    this.markersMap = new google.maps.Marker({
      position: new google.maps.LatLng(this.latPoint, this.lonPoint),
      icon: { url: 'assets/images/map/punto1.png' },
      map: this.map
    });


    this.fitAndGeofence();

    
  }

  cords2: any;

  fitAndGeofence() {
    let latlngbounds = new google.maps.LatLngBounds();
    let latlng = new google.maps.LatLng(this.latPoint, this.lonPoint)
    latlngbounds.extend(latlng);

    const coordsCircle = this.getCoordsCircleOfPoint(latlng, 0.03);
    this.drawCircleGeofence(coordsCircle.gCoords);
    this.map.fitBounds(latlngbounds);
  }

  drawCircleGeofence(coords: any) {
    this.geofencesMap = new google.maps.Polygon({
      map: this.map,
      paths: [coords],
      strokeColor: "#0000FF",
      strokeOpacity: 0.8,
      strokeWeight: 2,
      fillColor: "#FF0000",
      fillOpacity: 0.35,
      visible: true
    });
  }

  getCoordsCircleOfPoint(point, radius) {
    var d2r = Math.PI / 180;   // grados a radiantes
    // var r2d = 180 / Math.PI;   // radiantes a grados
    var earthsradius = 3963; // 3963 es el radio de la tierra en millas

    var points = 29; //puntos que contendra la geocerca

    // busqueda del radio en la lat/lon
    var rlat = (radius / earthsradius) * this.InterestPoint.Radius;
    var rlng = rlat / Math.cos(point.lat() * d2r);

    var gCoords = new Array();
    var rawCoords = new Array();

    var start=0;
    var end=points+1; // +1 extra para asegurar que se conecta la ruta

    for (var i=start; i < end; i=i+1)
    {
       var theta = Math.PI * (i / (points/2));
       const ey = point.lng() + (rlng * Math.cos(theta)); // centro a + radio x * cos(theta)
       const ex = point.lat() + (rlat * Math.sin(theta)); // centro b + radio y * sin(theta)
       gCoords.push(new google.maps.LatLng(ex, ey));
       rawCoords.push([ey, ex]);
    }


    return {
      gCoords: gCoords,
      rawCoords: rawCoords
    };
  }


}
