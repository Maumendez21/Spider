import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';

declare const google: any;

@Component({
  selector: 'app-nuevo-punto-interes',
  templateUrl: './nuevo-punto-interes.component.html',
  styleUrls: ['./nuevo-punto-interes.component.css']
})
export class NuevoPuntoInteresComponent implements OnInit {

  center: any = {
    lat: 19.4525976,
    lng: -99.1182164
  };

  map: any;

  radio: number;

  drawingManager: any;

  name: string = "";
  description: string = "";
  latitud: string = "";
  longitud: string = "";
  coordinates: any;

  markersMap: any;
  geofencesMap: any;

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService, private route: Router) {}

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

      self.eliminarPuntoInteres();

      const marker = new google.maps.Marker({
        position: new google.maps.LatLng(event.getPosition().lat(), event.getPosition().lng()),
        icon: { url: 'assets/images/punto1.png' },
        map: this.map
      });

      self.markersMap = marker;

      const coordsCircle = self.getCoordsCircleOfPoint(new google.maps.LatLng(event.getPosition().lat(), event.getPosition().lng()), 0.03);
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
    if (this.markersMap && this.geofencesMap) {
      this.markersMap.setMap(null);
      this.geofencesMap.setMap(null);
    }
  }

  registrarRuta() {

    if (this.name != "" && this.latitud != "" && this.longitud != "" && this.coordinates) {

      const data = {
        Name: this.name,
        Description: this.description,
        Latitude: this.latitud,
        Longitude: this.longitud,
        Radius: this.coordinates,
        Active: true
      };

      this.spiderService.setNuevoPuntoInteres(data)
        .subscribe(response => {
          if (response['success']) {
            this.toastr.success('Punto de interes registrado exitosamente', '¡Exito!');
            this.route.navigate(['/configuration/puntos-interes']);
          } else {
            this.toastr.warning('No se pudo registrar el Punto de Interes, intentalo nuevamente', 'Al parecer algo ocurrio');
          }
        });
    } else {
      this.toastr.warning("Es necesario nombrar y elegir un punto de interes", "Faltan campos");
    }
  }

}
