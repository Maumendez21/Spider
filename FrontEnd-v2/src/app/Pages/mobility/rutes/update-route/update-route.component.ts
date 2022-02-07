import { Component, OnInit } from '@angular/core';
import { MobilityService } from '../../services/mobility.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedService } from '../../../../Services/shared.service';
import Swal from 'sweetalert2';
declare var google: any;
@Component({
  selector: 'app-update-route',
  templateUrl: './update-route.component.html',
  styleUrls: ['./update-route.component.css']
})
export class UpdateRouteComponent implements OnInit {

  public center: any = {
    lat: 19.4525976,
    lng: -99.1182164
  };

  public map: any;

  public gruposList: any = [];
  public pointList: any = [];
  public selectedArea = 0;
  public drawingManager: any;

  public idRoute: string;
  public nameRoute: string;
  public descriptionRoute: string;
  public routes: any;
  public markersMap: any = [];
  public markersFit: any = [];


  constructor(
    private mobilityService: MobilityService,
    private router: Router,
    private route: ActivatedRoute,
    private shared: SharedService
  ) { 
    this.getRutaDetail();
    this.getRouteDevice();
  }

  ngOnInit(): void {
  }

  onMapReady(map: any) {
    this.map = map;
    // this.initializeDrawingManager(map);
  }

  route2: any = [];
  waypointsRoute2: any = [];
  waypointsRoute: any = [];

  // * Waypoints
  polylineTrip: any;
  polylineTrip2: any;

  public pageOfItems:Array<any>;

  eliminarRuta(index: string) {
    this.routes.splice(index, 1);
    this.markersMap[index].setMap(null);
    this.markersMap.splice(index, 1);
  }

  registrarRuta() {
    const data = {
      Id: this.idRoute,
      Name: this.nameRoute,
      Description: this.descriptionRoute,
      ListRoutes: this.routes
    };

    this.mobilityService.updateRuta(data)
      .subscribe(response => {
        if (response['success']) {
          Swal.fire({
            title: `${data.Name} Actualizada!`,
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
  }

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }
  

  getRutaDetail() {
    const id = this.route.snapshot.paramMap.get('id');

    // this.spiderService.getRutaConfiguracion('60ccc45b8b42930f783fee18')
    // .subscribe(ruta => {
    //   // this.idRoute = ruta.Id;
    //   // this.nameRoute = ruta.Name;
    //   // this.descriptionRoute = ruta.Description;
    //   this.route2 = ruta.ListPoints;
    //   // this.waypointsRoute2 = [];
    //   // ruta.ListPoints.map(function(x, i){
    //   //   this.waypointsRoute2.push({
    //   //     // lat: + x[1],
    //   //     // lng: + x[0]
    //   //     lat: + x[1],
    //   //     lng: + x[0]
    //   //   });

    //   // })

    //   console.log(this.waypointsRoute2);
    //   this.shared.broadcastRouteDirectionStream2(this.route2);
    //   // this.drawMarkersInMap();
    // });


    this.mobilityService.getRuta(id)
      .subscribe(ruta => {

        this.idRoute = ruta.Id;
        this.nameRoute = ruta.Name;
        this.descriptionRoute = ruta.Description;
        this.routes = ruta.ListPoints;



        this.shared.broadcastRouteDirectionStream(this.routes);

        this.drawMarkersInMap();
      });
  }

  getRouteDevice() {
    this.shared.routeDirectionStream$.subscribe(data => {
      const self = this;
      if (data) {

        this.waypointsRoute = [];
        data.map(function(x, i){
          self.waypointsRoute.push({
            lat: + x[1],
            lng: + x[0]
          });



        })
      }
    })
    // this.shared.routeDirectionStream2$.subscribe(data => {
    //   const self = this;
    //   if (data) {

    //     this.waypointsRoute2 = [];
    //     data.map(function(x, i){
    //       self.waypointsRoute2.push({
    //         lat: + x[1],
    //         lng: + x[0]
    //       });



    //     })

    //     console.log(self.waypointsRoute2);

    //   }
    // })
  }

  drawMarkersInMap() {

    const self = this;

    console.log(this.routes);
    

    this.routes.map(route => {
      /*COMENTAR SI SE NECESITA BORRAR LOS PUNTOS*/
      const marker = new google.maps.Marker({
        position: new google.maps.LatLng(route[1], route[0]),
        icon: { url: 'assets/images/map/punto1.png' },
        map: self.map
      });

      self.markersMap.push(marker);
      self.markersFit.push(new google.maps.LatLng(route[1], route[0]));
    });

    this.fitMap();

  }

  fitMap() {

    // console.log(this.waypointsRoute2);

    this.polylineTrip = new google.maps.Polyline({
      path: this.waypointsRoute,
      geodesic: true,
      strokeColor: '#42A5F5',
      strokeOpacity: 1.0,
      strokeWeight: 5
    });

    // this.polylineTrip2 = new google.maps.Polyline({
    //   path: this.waypointsRoute2,
    //   geodesic: true,
    //   strokeColor: '#770000',
    //   strokeOpacity: 1.0,
    //   strokeWeight: 5
    // });

    this.polylineTrip.setMap(this.map);
    // this.polylineTrip2.setMap(this.map);

    let latlngbounds = new google.maps.LatLngBounds();

    for (let i = 0; i < this.markersFit.length; i++) {
        latlngbounds.extend(this.markersFit[i]);
    }

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

      const marker = new google.maps.Marker({
        position: new google.maps.LatLng(event.getPosition().lat(), event.getPosition().lng()),
        icon: { url: 'assets/images/map/punto1.png' },
        map: this.map
      });

      // self.markersMap.push(marker);

      const coordsCircle = self.getCoordsCircleOfPoint(new google.maps.LatLng(event.getPosition().lat(), event.getPosition().lng()), 0.03);
      //self.drawCircleGeofence(coordsCircle.gCoords);

      const lon = event.getPosition().lng();
      const lat = event.getPosition().lat()

      // self.routes.push({
      //   Name: '',
      //   Latitude: event.getPosition().lat(),
      //   Longitude: event.getPosition().lng(),
      //   Notes: '',
      //   Coordinates: coordsCircle.rawCoords
      // });
      self.routes.push([lon, lat]);


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


  



}
