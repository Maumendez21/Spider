import { Component, OnInit } from '@angular/core';
import { MapService } from '../Services/map.service';
import { SharedService } from '../../../Services/shared.service';
import { Icons } from '../../../utils/iconos';
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

  constructor(
    private mapService: MapService,
    private shared: SharedService,
    private helperIcons: Icons,
  ) { 
    setInterval(() => {
      this.getDevices();
    }, 30000);
    this.icons = this.helperIcons.icons;
  }

  ngOnInit(): void {
  }

  onMapReady(map: any) {
    this.map = map;
    this.getDevices();
    // this.trafic = new google.maps.TrafficLayer();


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
          // if (this.clusterDynamicBtn) {
          //   this.shared.clusterDinamicoStream(true);
          // }
        }
      }, error => {
        // this.shared.broadcastLoggedStream(false);
        // this.shared.clearSharedSession();
        // this.route.navigate(['/login']);
      });
  }

  async clearOverlays() {

    // if (this.markerCluster) { this.markerCluster.clearMarkers(); }

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
    // this.getInfoDevice(device, estatus);
    // this.getInfoDevice2(device);
    // this.getParoMotorDevice(device);
    document.getElementById("btnModalInfoDevice").click();
  }


}
