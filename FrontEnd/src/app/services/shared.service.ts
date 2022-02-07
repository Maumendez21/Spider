import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  public logged: BehaviorSubject<boolean> = new BehaviorSubject(false);
  public name: BehaviorSubject<string> = new BehaviorSubject("N/D");
  public notifications: BehaviorSubject<string> = new BehaviorSubject("0");

  public rutaLogo: BehaviorSubject<boolean> = new BehaviorSubject(false);
  public spiderMarkers: BehaviorSubject<any> = new BehaviorSubject([]);
  public filterSubempresa: BehaviorSubject<any> = new BehaviorSubject({
    subempresa: "",
    search: false
  });
  public filterVehiculo: BehaviorSubject<any> = new BehaviorSubject({
    vehiculo: "",
    search: false
  });
  public filterEstatus: BehaviorSubject<any> = new BehaviorSubject({
    estatus: "",
    search: false
  });
  public filterType: BehaviorSubject<any> = new BehaviorSubject({
    typeV: "",
    search: false
  });
  public zoomCoords: BehaviorSubject<any> = new BehaviorSubject({
    device: "",
    latitud: 19.4525976,
    longitud: -99.1182164,
    zoom: 11,
    bottom: false,
    filterBottom: false,
    startDate: '',
    endDate: ''
  });
  public routeDirection: BehaviorSubject<any> = new BehaviorSubject([]);
  public routeDirection2: BehaviorSubject<any> = new BehaviorSubject([]);
  public listDevices: BehaviorSubject<any> = new BehaviorSubject([]);
  public permisos: BehaviorSubject<string> = new BehaviorSubject("");
  public analiticasGeneral: BehaviorSubject<any> = new BehaviorSubject([]);
  public clusterDinamico: BehaviorSubject<boolean> = new BehaviorSubject(true);
  public mapaDinamico: BehaviorSubject<boolean> = new BehaviorSubject(false);
  public mapaSatelite: BehaviorSubject<boolean> = new BehaviorSubject(false);
  public trafficLayer: BehaviorSubject<boolean> = new BehaviorSubject(false);

  public loggedStream$ = this.logged.asObservable();
  public nameStream$ = this.name.asObservable();
  public notificationsStream$ = this.notifications.asObservable();
  public logoStream$ = this.rutaLogo.asObservable();
  public spiderMarkersStream$ = this.spiderMarkers.asObservable();
  public filterSubempresaStream$ = this.filterSubempresa.asObservable();
  public filterVehiculoStream$ = this.filterVehiculo.asObservable();
  public filterEstatusStream$ = this.filterEstatus.asObservable();
  public filterTypeStream$ = this.filterType.asObservable();
  public zoomCoordsStream$ = this.zoomCoords.asObservable();
  public routeDirectionStream$ = this.routeDirection.asObservable();
  public routeDirectionStream2$ = this.routeDirection2.asObservable();
  public listDevicesStream$ = this.listDevices.asObservable();
  public permisosStream$ = this.permisos.asObservable();
  public analiticasGeneralStream$ = this.analiticasGeneral.asObservable();
  public clusterDinamicoStream$ = this.clusterDinamico.asObservable();
  public mapaDinamicoStream$ = this.mapaDinamico.asObservable();
  public mapaSateliteStream$ = this.mapaSatelite.asObservable();
  public trafficLayer$ = this.trafficLayer.asObservable();

  constructor(private route: Router) { }

  public broadcastLoggedStream(logged: boolean) {
    this.logged.next(logged);
  }

  public broadcastNameStream(name: string) {
    this.name.next(name);
  }
  public broadcastNotificationStream(notifications: string) {
    this.notifications.next(notifications);
  }

  public broadcastLogoStream(ruta: boolean){
    this.rutaLogo.next(ruta)
  }

  public broadcastSpiderMarkersStream(data: any) {
    this.spiderMarkers.next(data);
  }

  public broadcastFilterSubempresaStream(subempresa: any) {
    this.filterSubempresa.next(subempresa);
  }

  public broadcastFilterVehiculoStream(vehiculo: any) {
    this.filterVehiculo.next(vehiculo);
  }

  public broadcastFilterEstatusStream(estatus: any) {
    this.filterEstatus.next(estatus);
  }
  public broadcastFiltertypeStream(type: any) {
    this.filterType.next(type);
  }

  public broadcastZoomCoordsStream(zoomCoords: any) {
    this.zoomCoords.next(zoomCoords);
  }

  public broadcastRouteDirectionStream(routes: any) {
    this.routeDirection.next(routes);
  }
  public broadcastRouteDirectionStream2(routes: any) {
    this.routeDirection2.next(routes);
  }

  public broadcastListDevicesStream(listDevices: any) {
    this.listDevices.next(listDevices);
  }

  public broadcastPermisosStream(permisos: string) {
    this.permisos.next(permisos);
  }

  public broadcastAnaliticasGeneralStream(data: any) {
    this.analiticasGeneral.next(data);
  }

  public clusterDinamicoStream(active: boolean) {
    this.clusterDinamico.next(active);
  }
  public mapaDinamicoStream(active: boolean) {
    this.mapaDinamico.next(active);
  }
  public mapaSateliteStream(active: boolean) {
    this.mapaSatelite.next(active);
  }
  public trafficLayerStream(active: boolean) {
    this.trafficLayer.next(active);
  }

  public clearSharedSession() {

    localStorage.clear();

    this.logged.next(false);
    this.name.next("N/D");
  }

  public limpiarFiltros() {
    this.broadcastFilterSubempresaStream({
      subempresa: "",
      search: false
    });

    this.broadcastFilterVehiculoStream({
      vehiculo: "",
      search: false
    });

    this.broadcastRouteDirectionStream([]);

    this.broadcastZoomCoordsStream({
      device: "",
      latitud: 19.4525976,
      longitud: -99.1182164,
      zoom: 11,
      estatus: 0,
      bottom: false,
      filterBottom: false,
      startDate: '',
      endDate: ''
    });

    //console.log("Service", true);
    //this.clusterDinamicoStream(true);
  }

  public verifyLoggin() : boolean {

    let bool : boolean;

    this.loggedStream$.subscribe(data => {
      bool = data;
    });

    return bool;
  }

  signOut() {

    this.broadcastFilterSubempresaStream({
      subempresa: "",
      search: false
    });

    this.broadcastFilterVehiculoStream({
      vehiculo: "",
      search: false
    });

    this.broadcastFilterEstatusStream({
      estatus: "",
      search: false
    });

    this.broadcastFiltertypeStream({
      typeV: "",
      search: false
    })

    this.broadcastZoomCoordsStream({
      device: "",
      latitud: 19.4525976,
      longitud: -99.1182164,
      zoom: 11,
      bottom: false,
      filterBottom: false,
      startDate: '',
      endDate: ''
    });

    this.clearSharedSession();
    this.route.navigate(['/login']);

    const sidebar = document.getElementById("sidebarRight");

    if (!sidebar.classList.contains("active")) {
      sidebar.classList.toggle("active");
    }
  }

}
