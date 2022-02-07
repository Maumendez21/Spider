import { Component, OnInit } from '@angular/core';
import { MobilityService } from '../../services/mobility.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

declare const google: any;

@Component({
  selector: 'app-new-route',
  templateUrl: './new-route.component.html',
  styleUrls: ['./new-route.component.css']
})
export class NewRouteComponent implements OnInit {

  public center: any = {
    lat: 19.4525976,
    lng: -99.1182164
  };

  public map: any;

  public gruposList: any = [];
  public pointList: any = [];
  public selectedArea = 0;
  public drawingManager: any;

  public nameRoute: string;
  public descriptionRoute: string;
  public routes: any = [];
  public markersMap: any = [];

  constructor(
    private mobilityService: MobilityService,
    private router: Router
  ) { }

  ngOnInit(): void {
  }

  onMapReady(map) {
    this.map = map;
    this.initializeDrawingManager(map);
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

      const marker = new google.maps.Marker({
        position: new google.maps.LatLng(event.getPosition().lat(), event.getPosition().lng()),
        icon: { url: 'assets/images/map/punto1.png' },
        map: this.map
      });

      self.markersMap.push(marker);

      const coordsCircle = self.getCoordsCircleOfPoint(new google.maps.LatLng(event.getPosition().lat(), event.getPosition().lng()), 0.03);
      //self.drawCircleGeofence(coordsCircle.gCoords);

      const lon = event.getPosition().lng();
      const lat = event.getPosition().lat()


      // self.routes.push({
      //   Name: '',
      //   lon,
      //   lat
      //   Notes: '',
      //   Coordinates: coordsCircle.rawCoords
      // });
      self.routes.push([lon, lat]);
      console.log(self.routes);
      

    });
  }

  drawCircleGeofence(coords: any) {

    const _ = new google.maps.Polygon({
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
      rawCoords: rawCoords
    };
  }

  eliminarRuta(index: string) {
    this.routes.splice(index, 1);
    this.markersMap[index].setMap(null);
    this.markersMap.splice(index, 1);
  }

  registrarRuta() {
    const data = {
      Name: this.nameRoute,
      Description: this.descriptionRoute,
      Active: true,
      ListPoints: this.routes
    };


    if (data.Name !== "") {
      this.mobilityService.setNuevaRutaConfiguracion(data)
      .subscribe(response => {
        if (response['success']) {
          Swal.fire({
            title: 'Ruta registrada!',
            confirmButtonText: 'OK',
            icon: 'success'
          }).then((result) => {
            if (result.isConfirmed) {
              this.router.navigate(['/mobility/rutes']);
            }
          })
        } else {
          Swal.fire({
            icon: 'warning',
            title: 'Error: ' + response['messages'][0]
          })
        }
      });
    }else{
      Swal.fire({
        icon: 'error',
        title: 'La ruta debe tener un nombre'
      })
    }
  }
}
