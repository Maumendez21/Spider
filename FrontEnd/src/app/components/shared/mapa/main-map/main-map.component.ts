import { Component, OnInit } from '@angular/core';
import { InfoDevice } from 'src/app/models/info-device/info-device';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import { Router } from '@angular/router';
import { TravelMarker } from 'travel-marker';
import { Maps } from '../../../../helpers/maps';
import { Icons } from '../../../../helpers/iconos';
import MarkerClusterer from "@google/markerclusterer";
import { FilterSidebarRightPipe } from 'src/app/pipes/filter-sidebar-right.pipe';

declare var google: any;

@Component({
  selector: 'app-main-map',
  templateUrl: './main-map.component.html',
  styleUrls: ['./main-map.component.css']
})
export class MainMapComponent implements OnInit {

  company: string = localStorage.getItem("company");

  device: string;
  fechaInicio: string = "";
  fechaFin: string = "";

  travels: any;

  renderOptions = {
    suppressMarkers: true,
  }

  paroMotor: any = [];
  rutaLogo: string;
  alarmas: any[];

  waypointsMap: any = [];
  waypointsMapIcons: any = [];
  routes: any = [];

  // * Data Map
  spiderMarkers: any;
  subempresa: string = "";
  vehiculo: string = "";
  estatus: string = "";
  type: string = "";

  // * InfoWindow Modal
  infoDevice: InfoDevice;

  // * Config Map
  lat: number = 19.4525976;
  lng: number = -99.1182164;
  zoom: number = 11;


  // * FitBounds
  fitBounds: boolean = true;
  agmFitBounds: boolean = true;
  fitBoundsTrip: boolean = false;

  // * Waypoints
  polylineTrip: any;

  marker: TravelMarker = null;
  directionsService: any;
  line: any;
  map: any;
  style: any;
  icons: any;

  markersFit: any = [];
  markersMap: any = [];
  agmFit: boolean = true;
  intervalDevices: boolean = true;
  markerCluster: MarkerClusterer;
  waypointsRoute: any = [];
  waypointsIcons: any = [];

  notifications: string;

  clusterDynamicBtn: boolean = true;
  mapaDynamicBtn: boolean = false;
  SateliteBtn: boolean = false;

  mapId: string = 'roadmap';


  constructor(private helperIcons: Icons, private route: Router, private spider: SpiderfleetService, private shared: SharedService, private helperMap: Maps, private pipeSidebarRight: FilterSidebarRightPipe) {

    if (shared.verifyLoggin()) {

      setInterval(() => {

        if (shared.verifyLoggin()) {
          this.getDevices();
        }

      }, 30000);

      this.infoDevice = new InfoDevice();

      this.icons = helperIcons.icons;

      this.getFilters();
      this.getRouteDevice();
      this.getZoomCoords();
      this.clusterDynamicService();
      this.mapDynamicService();
      this.mapSatelit();
      // this.trafficLayer();
      this.shared.clusterDinamicoStream(true);
    } else {
      this.route.navigate(['/login']);
    }
  }

  mapDynamicService(){
    this.shared.mapaDinamicoStream$.subscribe(data => {
        this.mapaDynamicBtn = data;
        if (data) {
          this.style = [];
        }else{
          this.style = this.helperMap.style;
        }
    })
  }

  fontColor: string = "black";

  mapSatelit(){
    this.shared.mapaSateliteStream$.subscribe(data => {
      // console.log(data);
      this.SateliteBtn = data;
      if (data) {
        this.fontColor = "white"
        this.mapId = 'satellite'
      }else{

        this.fontColor = "black"
        this.mapId = 'roadmap'
      }

    })
  }



  ngOnInit(): void {}

  public trafic: any;

  onMapReady(map: any) {
    this.map = map;

    this.getDevices();
    this.trafic = new google.maps.TrafficLayer();
  }

  satelit1(){
    const trafficLayer = new google.maps.TrafficLayer();
    trafficLayer.setMap(null);
  }

  satelit(value: boolean){
    console.log(this.trafic);

    if (value) {
      this.trafic.setMap(this.map);
    } else {
      this.trafic.setMap(null);
    }
  }

  async getDevices() {
    await this.spider.getDevicesGeneralNew(this.vehiculo)
      .subscribe((data: any) => {
        if (this.intervalDevices) {

          this.clearOverlays();
          this.spiderMarkers = data['ListLastPosition'];
          // console.log(this.spiderMarkers);

          this.shared.broadcastSpiderMarkersStream(data['ListLastPosition']);
          this.drawMarkersDevices(data['ListLastPosition']);
          this.notifications = data['View'];
          this.shared.broadcastNotificationStream(this.notifications);
          if (this.clusterDynamicBtn) {
            this.shared.clusterDinamicoStream(true);
          }
        }
      }, error => {
        this.shared.broadcastLoggedStream(false);
        this.shared.clearSharedSession();
        this.route.navigate(['/login']);
      });
  }

  async drawMarkersDevices(spiderMarkers: any) {

    await spiderMarkers.map(device => {
      const marker = new google.maps.Marker({
        position: new google.maps.LatLng(device.latitud, device.longitud),
        title: device.nombre,
        label: { text: device.nombre, fontSize: '10', fontWeight: 'bold', color:  this.fontColor},
        icon: { url: this.icons[device.typeDevice][device.statusEvent].icon, scaledSize: new google.maps.Size(23, 35), labelOrigin: {x:15, y:-8} },
        map: this.map
      });

      marker.addListener("click", () => {
        this.map.setZoom(20);
        this.map.setCenter(marker.getPosition());
        this.lat = device.latitud;
        this.lng = device.longitud;
        this.openWindow(device.dispositivo, device.statusEvent);
        document.getElementById("btnModalInfoDevice").click();
      });

      this.markersFit.push(new google.maps.LatLng(device.latitud, device.longitud));
      this.markersMap.push(marker);
    });

    if (this.agmFit) {
      this.fitMap(this.markersFit);
      this.agmFit = false;
    }
  }

  async clearOverlays() {

    if (this.markerCluster) { this.markerCluster.clearMarkers(); }

    if (this.markersMap.length > 0) {

      const promises = this.markersMap.map(async marker => {
        await marker.setMap(null);
      });

      this.markersMap = [];
      this.markersFit = [];

      await Promise.all(promises);
    }
  }

  fitMap(markers: any) {

    let latlngbounds = new google.maps.LatLngBounds();

    for (let i = 0; i < markers.length; i++) {
        latlngbounds.extend(markers[i]);
    }

    this.map.fitBounds(latlngbounds);
  }



  getParoMotorDevice(device: string){
    this.spider.getEngineDevice(device).subscribe(data => {
      this.paroMotor = data;
    })
  }


  tipo: number = 0;
  infoDevice2: any = {};

  async getInfoDevice(device: string, estatus: number) {
    await this.spider.getInfoDevice(device)
      .subscribe(data => {
        this.tipo = data['Itineraries'].TypeDevice;

        this.rutaLogo = this.helperIcons.iconosFrente[data['Itineraries'].TypeDevice][estatus].icon;
        this.infoDevice = data['Itineraries'];
        this.alarmas = data['lastAlarms'];
        this.infoDevice['estatus'] = estatus;
      }, error => {
        this.shared.broadcastLoggedStream(false);
        this.shared.clearSharedSession();
        this.route.navigate(['/login']);
      });
  }

  async getInfoDevice2(device: string) {
    await this.spider.getInfoDevice2(device)
      .subscribe(data => {
        // console.log(data);
        this.infoDevice2 = data;

      }, error => {
        this.shared.broadcastLoggedStream(false);
        this.shared.clearSharedSession();
        this.route.navigate(['/login']);
      });
  }



  getTipoVehiculo = (type: number) => {
    switch(type) {
      case 1:
        return "Automóvil";
      case 2:
        return "Automóvil";
      case 3:
        return "Automóvil";
      case 4:
        return "Celda";
      case 5:
        return "Motocicleta";
      case 6:
        return "Dsipositivo TAG";
      case 7:
        return "Motocicleta";
    }
  }

  getEstatusName = (estatus: number) => {
    switch(estatus) {
      case 1:
        return "Activo";
      case 2:
        return "Inactivo";
      case 3:
        return "Sin comunicación";
      case 4:
        return "Pánico / Geocerca";
      case 5:
        return "Sin movimiento";
      case 6:
        return "Paro";
      case 7:
        return "Desconexión";
      case 10:
        return "Paro de motor";
    }
  }

  getFilters() {
    this.shared.filterSubempresaStream$.subscribe(data => {

      if (data.search) {

        this.subempresa = data.subempresa;
        this.clearOverlays();
        this.agmFit = true;

        if (data.subempresa != "") {
          this.intervalDevices = false;
          const markers = this.pipeSidebarRight.transform(this.spiderMarkers, data.subempresa, this.estatus, this.type);
          this.drawMarkersDevices(markers);
        } else {
          this.intervalDevices = true;
          this.drawMarkersDevices(this.spiderMarkers);
        }
      }
    });

    this.shared.filterVehiculoStream$.subscribe(data => {

      if (data.search) {
        this.vehiculo = data.vehiculo;
        this.intervalDevices = true;
        this.agmFit = true;
        this.getDevices();
      }
    });

    this.shared.filterEstatusStream$.subscribe(data => {

      if (data.search) {

        this.estatus = data.estatus;
        this.clearOverlays();
        this.agmFit = true;

        if (data.estatus != "") {
          this.intervalDevices = false;
          const markers = this.pipeSidebarRight.transform(this.spiderMarkers, this.subempresa, data.estatus, this.type);
          this.drawMarkersDevices(markers);
        } else {
          this.intervalDevices = true;
          const markers = this.pipeSidebarRight.transform(this.spiderMarkers, this.subempresa, "", this.type);
          this.drawMarkersDevices(markers);
        }
      }
    });


    this.shared.filterTypeStream$.subscribe(data => {


      // console.log(data);


      if (data.search) {

        this.type = data.typeV;
        this.clearOverlays();
        this.agmFit = true;

        if (data.typeV != "") {
          this.intervalDevices = false;
          const markers = this.pipeSidebarRight.transform(this.spiderMarkers, this.subempresa, this.estatus, data.typeV);
          this.drawMarkersDevices(markers);
        } else {
          this.intervalDevices = true;
          const markers = this.pipeSidebarRight.transform(this.spiderMarkers, this.subempresa, this.estatus, "");
          this.drawMarkersDevices(markers);
        }
      }
    });
  }

  getZoomCoords() {
    this.shared.zoomCoordsStream$.subscribe(data => {

      if (data['device']) {

        this.lat = data["latitud"];
        this.lng = data["longitud"];
        this.map.setZoom(20);
        this.map.setCenter(new google.maps.LatLng(data["latitud"], data["longitud"]));

        this.device = data['device'];
        this.getInfoDevice(data['device'], data['estatus']);
        this.getInfoDevice2(data['device']);
        this.getParoMotorDevice(data['device']);
      }

    });
  }

  clearAll() {
    this.subempresa = "";
    this.estatus = "";
    this.vehiculo = "";
    this.intervalDevices = true;
    this.agmFit = true;
    this.clearPolylines();
    this.getDevices();
  }

  async clearPolylines() {
    if (this.waypointsRoute.length > 0) {
      await this.polylineTrip.setMap(null);
    }
  }

  getRouteDevice() {
    this.shared.routeDirectionStream$.subscribe(data => {

      if (data.listPoints) {

        const self = this;
        this.intervalDevices = false;

        this.clearPolylines();
        this.clearOverlays();

        this.setMarker('assets/images/start2.png', +data.listPoints[0].lat, +data.listPoints[0].lng);
        this.setMarker('assets/images/finish.png', +data.listPoints[ data.listPoints.length - 1 ].lat, +data.listPoints[ data.listPoints.length - 1 ].lng);

        this.waypointsRoute = [];

        data.listPoints.map(function(x, i) {
          self.waypointsRoute.push({
            lat: +x.lat,
            lng: +x.lng
          });
        });

        data.listWaitTime.map(function(x, i) {
          switch (x.events) {
            case "Hard Deceleration":
              self.setMarker('assets/images/frenos.png', +x.lat, +x.lng);
              break;
            case "Hard Acceleration":
              self.setMarker('assets/images/aceleracion.png', +x.lat, +x.lng);
              break;
            case "High RPM":
              self.setMarker('assets/images/rpm.png', +x.lat, +x.lng);
              break;
            case "Speeding":
              self.setMarker('assets/images/velocidad.png', +x.lat, +x.lng);
              break;
            case "Speed Excess":
              self.setMarker('assets/images/velocidad.png', +x.lat, +x.lng);
            case "Wait Time":
              self.setMarker('assets/images/inactividad.png', +x.lat, +x.lng);
            default:
              break;
          }
        });

        this.initDrawPolylinesShip();
      }

    });
  }

  setMarker(url: string, latitud: number, longitud: number) {
    const marker = new google.maps.Marker({
      position: new google.maps.LatLng(latitud, longitud),
      icon: { url: url, labelOrigin: {x:15, y:-8} },
      map: this.map
    });

    this.markersFit.push(new google.maps.LatLng(latitud, longitud));
    this.markersMap.push(marker);
  }

  initDrawPolylinesShip() {

    this.polylineTrip = new google.maps.Polyline({
      path: this.waypointsRoute,
      geodesic: true,
      strokeColor: "#42A5F5",
      strokeOpacity: 1.0,
      strokeWeight: 5,
    });

    this.polylineTrip.setMap(this.map);
    this.fitMap(this.waypointsRoute);
  }

  setDevice = (device: string, estatus: string) => {
    this.device = device;
    this.shared.broadcastZoomCoordsStream({
      device: device,
      estatus: estatus,
      latitud: this.lat,
      longitud: this.lng,
      zoom: this.zoom,
      bottom: true,
      filterBottom: false,
      startDate: '',
      endDate: ''
    });
  }

  prepareDates = (inicio: string, fin: string) => {

    this.fechaInicio = inicio;
    this.fechaFin = fin;

    this.searchAllTrips();
  }

  searchAllTrips() {

    if (this.fechaInicio && this.fechaFin) {

      this.shared.broadcastZoomCoordsStream({
        device: this.device,
        latitud: this.lat,
        longitud: this.lng,
        zoom: this.zoom,
        bottom: true,
        filterBottom: true,
        startDate: this.fechaInicio,
        endDate: this.fechaFin
      });
    }
  }

  openWindow(device: string, estatus: number) {
    this.device = device;
    this.getInfoDevice(device, estatus);
    this.getInfoDevice2(device);
    this.getParoMotorDevice(device);
    document.getElementById("btnModalInfoDevice").click();
  }



  clusterDynamicService() {
    this.shared.clusterDinamicoStream$.subscribe(data => {

      this.clusterDynamicBtn = data;

      if (this.markerCluster) { this.markerCluster.clearMarkers(); }

      if (data && this.markersMap.length > 0) {
        this.markerCluster = new MarkerClusterer(this.map, this.markersMap, this.helperMap.mcOptions);
      } else if (!data && this.markerCluster) {
        this.clearOverlays();
        this.drawMarkersDevices(this.spiderMarkers);
      }
    });
  }

}
