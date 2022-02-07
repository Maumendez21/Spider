import { Component, OnInit } from '@angular/core';
import { SharedService } from 'src/app/Services/shared.service';
import { MapService } from '../Services/map.service';
// import MarkerClusterer from "@google/markerclusterer";
import { Icons } from '../../../utils/iconos';
import { InfoDevice } from '../models/info-device';
import { Router } from '@angular/router';
import MarkerClusterer from "@google/markerclusterer";
import { FilterSidebarRightPipe } from 'src/app/Pipes/filter-sidebar-right.pipe';
import { Maps } from 'src/app/utils/maps';
declare var google: any;

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit {

  public markersFit: any = [];
  public markersMap: any = [];
  public agmFit: boolean = true;
  public intervalDevices: boolean = true;

  public notifications: string;
  public device: string;


  public map: any;
  public style: any;
  public icons: any;

  // * Data Map
  public spiderMarkers: any;
  public vehiculo: string = "";
  public subempresa: string = "";
  public estatus: string = "";
  public type: string = "";

  // * Config Map
  public lat: number = 19.4525976;
  public lng: number = -99.1182164;
  public zoom: number = 11;

  // * InfoWindow Modal
  public infoDevice: InfoDevice;
  public tipo: number = 0;
  public rutaLogo: string;
  public alarmas: any[];
  public paroMotor: any = [];
  public infoDevice2: any = {};


  // clusters
  public markerCluster: MarkerClusterer;
  public clusterDynamicBtn: boolean = true;


  public mapaDynamicBtn: boolean = false;

  public SateliteBtn: boolean = false;
  public mapId: string = 'roadmap';


  public idu = "";
  constructor(
    private mapService: MapService,
    private shared: SharedService,
    private helperIcons: Icons,
    private router: Router,
    private pipeSidebarRight: FilterSidebarRightPipe,
    private helperMap: Maps

  ) {

    this.infoDevice = new InfoDevice();
    setInterval(() => {
      this.getDevices();
    }, 30000);
    this.icons = this.helperIcons.icons;
    this.getRouteDevice();
    this.btnTravels();
    this.getFilters();
    this.getZoomCoords();
    this.clusterDynamicService();
    this.mapDynamicService();
    this.mapSatelit();
    this.idu = localStorage.getItem("idu");
  }

  checkIdu(): boolean{
    switch (this.idu) {
      case '73':
        return false
      case '80':
        return false
      case '81':
        return false
      case '81-10':
        return false
      default:
        return true
    }
  }

  ngOnInit(): void {

    this.shared.broadcastPermisosStream('MAP100');
  }

  public trafic: any;

  onMapReady(map: any) {
    this.map = map;
    this.getDevices();
    this.trafic = new google.maps.TrafficLayer();


  }

  mapSatelit(){
    this.shared.mapaSateliteStream$.subscribe(data => {
      // console.log(data);
      this.SateliteBtn = data;
      if (data) {
        // this.fontColor = "white"
        this.mapId = 'satellite'
      }else{

        // this.fontColor = "black"
        this.mapId = 'roadmap'
      }

    })
  }

  satelit(value: boolean) {
    console.log(this.trafic);
    if (value) {
      this.trafic.setMap(this.map);
    } else {
      this.trafic.setMap(null);
    }
  }

  async getDevices() {
    await this.mapService.getDevicesGeneralNew(this.vehiculo)
      .subscribe((data: any) => {
        if (this.intervalDevices) {

          this.clearOverlays();
          this.spiderMarkers = data['ListLastPosition'];

          this.shared.broadcastSpiderMarkersStream(data['ListLastPosition']);
          this.drawMarkersDevices(data['ListLastPosition']);
          this.notifications = data['View'];
          this.shared.broadcastNotificationStream(this.notifications);
          if (this.clusterDynamicBtn) {
            this.shared.clusterDinamicoStream(true);
          }
        }
      }, error => {
        // this.shared.broadcastLoggedStream(false);
        // this.shared.clearSharedSession();
        // this.route.navigate(['/login']);
      });
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


  async drawMarkersDevices(spiderMarkers: any) {
        
    await spiderMarkers.map(device => {
      const marker = new google.maps.Marker({
        position: new google.maps.LatLng(device.latitud, device.longitud),
        title: device.nombre,
        label: { text: device.nombre, fontSize: '10', fontWeight: 'bold', color:  'black'},
        icon: { url: this.icons[device.typeDevice][device.statusEvent].icon, scaledSize: new google.maps.Size(30, 30), labelOrigin: {x:15, y:-8} },
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

  fitMap(markers: any) {

    const latlngbounds = new google.maps.LatLngBounds();
    for (let i = 0; i < markers.length; i++) {
      latlngbounds.extend(markers[i]);
    }

    this.map.fitBounds(latlngbounds);
  }

  openWindow(device: string, estatus: number) {
    this.device = device;
    this.getInfoDevice(device, estatus);
    // this.getInfoDevice2(device);
    // this.getParoMotorDevice(device);
    document.getElementById("btnModalInfoDevice").click();
  }

  

  async getInfoDevice(device: string, estatus: number) {
    await this.mapService.getInfoDevice(device)
      .subscribe(data => {
        this.tipo = data['Itineraries'].TypeDevice;

        this.rutaLogo = this.helperIcons.iconosFrente[data['Itineraries'].TypeDevice][estatus].icon;
        this.infoDevice = data['Itineraries'];
        this.alarmas = data['lastAlarms'];
        this.infoDevice['estatus'] = estatus;
      }, error => {
        this.shared.broadcastLoggedStream(false);
        this.shared.clearSharedSession();
        this.router.navigate(['/login']);
      });
  }

  async getInfoDevice2(device: string) {
    await this.mapService.getInfoDevice2(device)
      .subscribe(data => {
        this.infoDevice2 = data;
      }, error => {
        this.shared.broadcastLoggedStream(false);
        this.shared.clearSharedSession();
        this.router.navigate(['/login']);
      });
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

  setDevice = (device: string, estatus: string) => {

    this.shared.broadcastRightMenuStream({
      title: "Viajes",
      tipe: false
    })

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

  waypointsRoute: any = [];
  // * Waypoints
  polylineTrip: any;

  async clearPolylines() {
    if (this.waypointsRoute.length > 0) {
      await this.polylineTrip.setMap(null);
    }
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

  getRouteDevice() {
    this.shared.routeDirectionStream$.subscribe(data => {

      if (data.listPoints) {

        const self = this;
        this.intervalDevices = false;

        this.clearPolylines();
        this.clearOverlays();

        this.setMarker('assets/images/map/start2.png', +data.listPoints[0].lat, +data.listPoints[0].lng);
        this.setMarker('assets/images/map/finish.png', +data.listPoints[ data.listPoints.length - 1 ].lat, +data.listPoints[ data.listPoints.length - 1 ].lng);

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
              self.setMarker('assets/images/map/frenos.png', +x.lat, +x.lng);
              break;
            case "Hard Acceleration":
              self.setMarker('assets/images/map/aceleracion.png', +x.lat, +x.lng);
              break;
            case "High RPM":
              self.setMarker('assets/images/map/rpm.png', +x.lat, +x.lng);
              break;
            case "Speeding":
              self.setMarker('assets/images/map/velocidad.png', +x.lat, +x.lng);
              break;
            case "Speed Excess":
              self.setMarker('assets/images/map/velocidad.png', +x.lat, +x.lng);
            case "Wait Time":
              self.setMarker('assets/images/map/inactividad.png', +x.lat, +x.lng);
            default:
              break;
          }
        });

        this.initDrawPolylinesShip();
      }

    });
  }

  initDrawPolylinesShip() {

    this.polylineTrip = new google.maps.Polyline({
      path: this.waypointsRoute,
      geodesic: true,
      strokeColor: "#727cf5",
      strokeOpacity: 1.0,
      strokeWeight: 5,
    });

    this.polylineTrip.setMap(this.map);
    this.fitMap(this.waypointsRoute);
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


  public tipe: boolean = true;

  btnTravels() {
    this.shared.rightMenu$.subscribe(data => {

      this.tipe = data['tipe'];

    })
  }

  getParoMotorDevice(device: string){
    this.mapService.getEngineDevice(device).subscribe(data => {
      this.paroMotor = data;
    })
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



}
