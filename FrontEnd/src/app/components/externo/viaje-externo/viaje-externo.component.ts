import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Maps } from 'src/app/helpers/maps';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import {Label } from 'ng2-charts';
import { ChartOptions } from 'chart.js';
declare var google: any;

@Component({
  selector: 'app-viaje-externo',
  templateUrl: './viaje-externo.component.html',
  styleUrls: ['./viaje-externo.component.css']
})
export class ViajeExternoComponent implements OnInit {

  nombre: string;
  startDate : string;
  endDate: string;
  startingPoint: string;
  finalPoint: string;

  device: string;
  fechaInicio: string;
  fechaFin: string;

  waypointsMap: any = [];
  waypointsMapIcons: any = [];
  routes: any = [];
  listTime: any = [];
  listWaitTime: any = [];

  // * Config Map
  lat: number = 19.4525976;
  lng: number = -99.1182164;
  zoom: number = 11;
  style: any;

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

  iconWaitTime: any = {
    'Speed Excess': 'assets/images/velocidad.png',
    'Wait Time': 'assets/images/inactividad.png',
    'ENG': 'assets/images/frenos.png',
  };




  iconTime: string = "assets/images/pinTime.png";

  // * FitBounds
  fitBounds: boolean = true;
  agmFitBounds: boolean = true;

  // * Waypoints
  polylineTrip: any;

  //marker: TravelMarker = null;
  directionsService: any;
  line: any;
  map: any;

  data: any;

  // * Marker Animation
  play: boolean = true;
  timer: any;
  marker: any;
  markerAnimate: any;
  stopAnimation: boolean = false;
  infoWindow: any;


  fuel: string;
  time: string;
  distance: string;

  aceleration: string;
  braking: string;
  speed: string;

  iconAnimation = {
    path: "M17.402,0H5.643C2.526,0,0,3.467,0,6.584v34.804c0,3.116,2.526,5.644,5.643,5.644h11.759c3.116,0,5.644-2.527,5.644-5.644 V6.584C23.044,3.467,20.518,0,17.402,0z M22.057,14.188v11.665l-2.729,0.351v-4.806L22.057,14.188z M20.625,10.773 c-1.016,3.9-2.219,8.51-2.219,8.51H4.638l-2.222-8.51C2.417,10.773,11.3,7.755,20.625,10.773z M3.748,21.713v4.492l-2.73-0.349 V14.502L3.748,21.713z M1.018,37.938V27.579l2.73,0.343v8.196L1.018,37.938z M2.575,40.882l2.218-3.336h13.771l2.219,3.336H2.575z M19.328,35.805v-7.872l2.729-0.355v10.048L19.328,35.805z",
    scale: .7,
    strokeColor: 'white',
    strokeWeight: .10,
    fillOpacity: 1,
    fillColor: '#404040',
    offset: '5%',
    rotation: '',
    anchor: ''
  };

  constructor(private route: ActivatedRoute, private shared: SharedService, private spiderService: SpiderfleetService, private router: Router, private mapHelper: Maps) {

    if (this.shared.verifyLoggin()) {

      this.limpiarFiltrosMapa();

      route.params.subscribe(params => {
        this.device = params['device'];
        this.fechaInicio = params['startDate'];
        this.fechaFin = params['endDate'];
        this.style = mapHelper.style;
      });

    } else {
      localStorage.setItem("trip", window.location.href);
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

  onMapReady(map: any) {
    this.map = map;
    this.getRouteDevice();
  }


  public dataSet: any;
  public labels: Label[] = [];
  public limit: any = 0;

  public type: number;

  responsible: string;

  getRouteDevice() {

    this.spiderService.getTripLink(this.device, this.fechaInicio, this.fechaFin)
      .subscribe((response:any) => {

        this.dataSet = response.Grafica;
        this.labels = response.Grafica.label;
        this.limit = response.Grafica.MaximumSpeed;

        this.type = response.DeviceType;

        this.data = response;

        this.aceleration = response.Acceleration;
        this.speed = response.Speed;
        this.braking = response.Braking;

        this.fuel = response.FuelConsumption;
        this.time = response.ElapsedTime;
        this.distance = response.OdoConsumption;
        this.responsible = response.ResponsibleName;

        this.listTime = response['listTime'];
        this.listWaitTime = response['listWaitTime'];
        this.nombre = this.data.VehicleName;
        this.startDate = this.data.StartDate;
        this.endDate = this.data.EndDate;
        this.startingPoint = this.data.StartingPoint;
        this.finalPoint = this.data.FinalPoint;
        this.drawRoute();
      });
  }

  drawRoute() {

    this.routes = [];
    this.waypointsMap = [];
    this.waypointsMapIcons = [];

    let waypointsRoute: any = [];
    let waypointsIcons: any = [];

    const self = this;

    if (this.routes.length < 1) {

      if (this.data.listPoints) {
        if (this.data.listPoints != null) {

          this.data.listPoints.map(function(x, i) {

            if (i == 0) {
              waypointsIcons.push({
                icon: { url: 'assets/images/start2.png' },
                position: {
                  lat: +x.lat,
                  lng: +x.lng
                }
              });
            }

            waypointsRoute.push({
              lat: +x.lat,
              lng: +x.lng
            });

            if (i == self.data.listPoints.length - 2) {
              waypointsIcons.push({
                icon: { url: 'assets/images/finish.png' },
                position: {
                  lat: +x.lat,
                  lng: +x.lng
                }
              });
            }

            if (i == self.data.listPoints.length - 1) {
              waypointsIcons.push({
                //icon: { url: 'assets/images/finish.png' },
                icon: { url: 'assets/images/iconApagado.png' },
                position: {
                  lat: +x.lat,
                  lng: +x.lng
                }
              });
            }
          });

          this.data.listIcons.map(function(x, i) {

            switch (x.Name) {
              case "Hard Deceleration":
                waypointsIcons.push({
                  icon: { url: 'assets/images/frenos.png' },
                  position: {
                    lat: +x.lat,
                    lng: +x.lng
                  }
                });
                break;
              case "Hard Acceleration":
                waypointsIcons.push({
                  icon: { url: 'assets/images/aceleracion.png' },
                  position: {
                    lat: +x.lat,
                    lng: +x.lng
                  }
                });
                break;
              case "High RPM":
                waypointsIcons.push({
                  icon: { url: 'assets/images/rpm.png' },
                  position: {
                    lat: +x.lat,
                    lng: +x.lng
                  }
                });
                break;
              case "Speeding":
                waypointsIcons.push({
                  icon: { url: 'assets/images/velocidad.png' },
                  position: {
                    lat: +x.lat,
                    lng: +x.lng
                  }
                });
                break;
              default:
                break;
            }

          });

          this.routes.push({
            waypointsRoute: waypointsRoute,
            waypointsIcons: waypointsIcons
          });

          this.waypointsMap = waypointsRoute;
          this.waypointsMapIcons = waypointsIcons;

          this.initDrawPolylinesShip();
        }

      }

    }

  }

  initDrawPolylinesShip() {
    this.polylineTrip = new google.maps.Polyline({
      path: this.routes[0]["waypointsRoute"],
      geodesic: true,
      strokeColor: "#42A5F5",
      strokeOpacity: 1.0,
      strokeWeight: 5,
      editable: false
    });

    this.polylineTrip.setMap(this.map);
  }

  playMarker() {
    this.btnPlay();
    this.play = false;




  }

  stopMarker() {
    this.btnStop();
    this.play = true;
  }

  pauseMarker() {
    this.btnPause();
    this.play = true;
  }

  clearMarker() {
    this.marker.setMap(null);
  }

  btnPlay() {

    if (this.marker) {
      this.clearMarker();
    }

    this.marker = new google.maps.Marker({
      map: this.map,
    });

    if (!this.timer) {
      this.recursiveAnimate(0)
    } else {
      this.timer.resume()
    }
    this.marker.setMap(this.map)
  }

  btnPause() {
    /*this.infoWindow = new google.maps.InfoWindow({
      content: "Otro Texto"
    });*/
    this.timer && this.timer.pause()
    //this.infoWindow.open(this.map, this.marker);
  }

  btnStop() {
    this.clearMarker();
    this.timer && this.timer.cancel();
    this.timer = null;
    this.marker.setMap(null)
    //this.infoWindow.close();
  }

  recursiveAnimate(index) {
    this.timer && this.timer.cancel()
    var coordsDeparture = this.routes[0]["waypointsRoute"][index];
    var coordsArrival = this.routes[0]["waypointsRoute"][index + 1];

    var departure = new google.maps.LatLng(coordsDeparture.lat, coordsDeparture.lng);
    var arrival = new google.maps.LatLng(coordsArrival.lat, coordsArrival.lng);
    var step = 0;
    var numSteps = 20;
    var timePerStep = 3;


    this.timer = this.InvervalTimer((arg) => {
      step += 1;

      if (step > numSteps) {

        step = 0


        this.timer.cancel()
        if (index < this.routes[0]["waypointsRoute"].length - 2) {
          this.recursiveAnimate(index + 1)
        }
      } else {
        var are_we_there_yet = google.maps.geometry.spherical.interpolate(departure, arrival, step / numSteps);
        this.moveMarker(departure, are_we_there_yet)
        // console.log(departure);



      }
    }, timePerStep, null);

  }

  infoWindowOpened = null;

  showInfoWindow(event: any){

    if (this.infoWindowOpened === event) {
      return;
    }

    if (this.infoWindowOpened !== null) {
      this.infoWindowOpened.close();
    }

    this.infoWindowOpened = event;
  }

  moveMarker(departure, currentMarkerPos) {
    this.marker.setPosition(currentMarkerPos);
    this.map.panTo(currentMarkerPos);

    var heading = google.maps.geometry.spherical.computeHeading(departure, currentMarkerPos);
    this.iconAnimation.rotation = heading;
    this.iconAnimation.anchor = new google.maps.Point(10, 25);
    this.marker.setIcon(this.iconAnimation);
  }

  InvervalTimer(callback, interval, arg) {
    let timerId, startTime, remaining = 0;
    var state = 0; //  0 = idle, 1 = running, 2 = paused, 3= resumed
    var timeoutId
    let pause = () => {
      if (state != 1) return;
      remaining = interval - (new Date().getTime() - startTime);
      window.clearInterval(timerId);
      state = 2;
    };

    let resume = () => {
      if (state != 2) return;

      state = 3;
      timeoutId = window.setTimeout(timeoutCallback, remaining, arg);
    };

    let timeoutCallback = (timer) => {
      if (state != 3) return;
      clearTimeout(timeoutId);
      startTime = new Date();
      timerId = window.setInterval(function() {
        callback(arg)
      }, interval);
      state = 1;
    };

    let cancel = () => {
      clearInterval(timerId)
    }
    startTime = new Date();
    timerId = window.setInterval(function() {
      callback(arg)
    }, interval);
    state = 1;
    return {
      cancel: cancel,
      pause: pause,
      resume: resume,
      timeoutCallback: timeoutCallback
    };
  }
}
