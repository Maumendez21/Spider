import { Component, OnInit } from '@angular/core';
import { MobilityService } from '../../services/mobility.service';
import { Router, ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';


declare const google: any;

@Component({
  selector: 'app-action-point-interest',
  templateUrl: './action-point-interest.component.html',
  styleUrls: ['./action-point-interest.component.css']
})
export class ActionPointInterestComponent implements OnInit {

  center: any = {
    lat: 19.4525976,
    lng: -99.1182164
  };

  map: any;

  drawingManager: any;

  id: string = "";
  name: string = "";
  description: string = "";
  latitud: number = 0;
  longitud: number = 0;

  coordinates: any;

  radio: number;

  markersMap: any;
  geofencesMap: any;

  title: string;


  constructor(
    private mobilityService: MobilityService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
  }

  onMapReady(map) {
    this.map = map;
    this.initializeDrawingManager(map);
    this.getPuntoInteres();
  }

  getPuntoInteres() {
    this.id = this.route.snapshot.paramMap.get('id');

    if (this.id !== 'new') {

      this.title = 'Actualizar';
      
      this.mobilityService.getPuntoInteres(this.id)
        .subscribe(punto => {
          
          this.name = punto.Name;
          this.description = punto.Description;
          this.latitud = +punto.Latitude;
          this.longitud = +punto.Longitude;
          this.radio = punto.Radius;
          
          this.drawMarkersInMap();
        });
      }else{
      this.title = 'Crear';
      return;
    }

  }

  drawMarkersInMap() {

    this.markersMap = new google.maps.Marker({
      position: new google.maps.LatLng(this.latitud, this.longitud),
      icon: { url: 'assets/images/map/punto1.png' },
      map: this.map
    });

    this.fitAndGeofence();
  }

  fitAndGeofence() {
    let latlngbounds = new google.maps.LatLngBounds();
    let latlng = new google.maps.LatLng(this.latitud, this.longitud)
    latlngbounds.extend(latlng);

    const coordsCircle = this.getCoordsCircleOfPoint(latlng, 0.03);
    this.drawCircleGeofence(coordsCircle.gCoords);

    this.map.fitBounds(latlngbounds);
  }

  initializeDrawingManager(map: any) {

    const marker = {
      draggable: false,
      visible: false
    };

    const options = {
      drawingControl: true,
      drawingControlOptions: {
        drawingModes: ['marker'],
      },
      markerOptions: marker,
      drawingMode: google.maps.drawing.OverlayType.MARKER,
    };

    this.drawingManager = new google.maps.drawing.DrawingManager(options);
    this.drawingManager.setMap(map);

    
      
    this.initializeMarkersComplete();
    

  }

  initializeMarkersComplete() {

    const self = this;
    

    google.maps.event.addListener(this.drawingManager, "markercomplete", function(event) {

      self.eliminarPuntoInteres();
      let coordsCircle = null;

      if (self.id !== 'new') {
        
        self.markersMap = new google.maps.Marker({
          position: new google.maps.LatLng(event.getPosition().lat(), event.getPosition().lng()),
          icon: { url: 'assets/images/map/punto1.png' },
          map: this.map
        });

         coordsCircle = self.getCoordsCircleOfPoint(new google.maps.LatLng(event.getPosition().lat(), event.getPosition().lng()), 0.03);
         
        }else {
          
          const marker = new google.maps.Marker({
            position: new google.maps.LatLng(event.getPosition().lat(), event.getPosition().lng()),
            icon: { url: 'assets/images/map/punto1.png' },
            map: this.map
          });
          
          self.markersMap = marker;
          coordsCircle = self.getCoordsCircleOfPoint2(new google.maps.LatLng(event.getPosition().lat(), event.getPosition().lng()), 0.03);
      }  
      self.drawCircleGeofence(coordsCircle.gCoords);

      self.latitud = event.getPosition().lat();
      self.longitud = event.getPosition().lng();
      self.coordinates = coordsCircle.rawCoords;

    });
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
    var rlat = (radius / earthsradius) * this.radio;
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

  getCoordsCircleOfPoint2(point, radius) {
    var d2r = Math.PI / 180;   // grados a radiantes
    var r2d = 180 / Math.PI;   // radiantes a grados
    var earthsradius = 3963; // 3963 es el radio de la tierra en millas

    var points = 29; //puntos que contendra la geocerca

    // busqueda del radio en la lat/lon
    var rlat = (radius / earthsradius) * r2d;
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
      rawCoords: r2d
    };
  }

  eliminarPuntoInteres() {

    if (this.markersMap) {
      this.markersMap.setMap(null);
    }

    if (this.geofencesMap) {
      this.geofencesMap.setMap(null);
    }
  }

  actualizarPunto() {

    if (this.name != "" && this.latitud != 0 && this.longitud != 0 && this.coordinates) {

      
      if (this.id !== 'new') {
        
        const data = {
          Id: this.id,
          Name: this.name,
          Description: this.description,
          Latitude: this.latitud,
          Longitude: this.longitud,
          Radius: this.radio,
        };
        this.mobilityService.updatePuntoInteres(data)
          .subscribe(response => {
            if (response['success']) {
              Swal.fire({
                title: 'Punto de interés Actualizado!',
                confirmButtonText: 'OK',
                icon: 'success'
              }).then((result) => {
                if (result.isConfirmed) {
                  this.router.navigate(['/mobility/points-interest']);
                }
              })
            } else {
              Swal.fire('ERROR', 'No se pudo actualizar el Punto de Interes, intentalo nuevamente', 'warning')
            }
          });
      }else {


        const data = {
          Name: this.name,
          Description: this.description,
          Latitude: this.latitud,
          Longitude: this.longitud,
          Radius: this.coordinates,
          Active: true
        };


        this.mobilityService.setNuevoPuntoInteres(data)
        .subscribe(response => {
          if (response['success']) {
            Swal.fire({
              title: 'Punto de interés registrado!',
              confirmButtonText: 'OK',
              icon: 'success'
            }).then((result) => {
              if (result.isConfirmed) {
                this.router.navigate(['/mobility/points-interest']);
              }
            })
          } else {
            Swal.fire('ERROR', 'No se pudo registrar el Punto de Interes, intentalo nuevamente', 'warning')
          }
        });
      }
    } else {
      Swal.fire('ERROR', 'Es necesario nombrar y elegir un punto de interes', 'error')
    }
  }





}
