import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { ServiceClass } from '../../../utils/services-class';

@Injectable({
  providedIn: 'root'
})
export class GeofencesService {


  public serviceClass = null;

  constructor(
    private http: HttpClient,
  ) {
    this.serviceClass = new ServiceClass(this.http);
  }

  getListGeocercasMonitoreo() {
    return this.serviceClass.getQuery(`geo/fence/monitoring/list`)
      .pipe( map(data => {
        return data['ListGeoFences'];
      }));
  }
  getListGeocercasHistorico(device: string, geo: string, fechaIni: string, fechaFin: string) {
    return this.serviceClass.getQuery(`geo/fence/history/list?device=${device}&mongo=${geo}&start=${fechaIni}&end=${fechaFin}`)
      .pipe( map(data => {
      return data['ListPointsTimeOut'];
    }));
  }

  getDevicesGeofenceMonitoreo(id: string) {
    return this.serviceClass.getQuery(`geo/fence/monitoring/last/positions?id=${id}`)
      .pipe( map(data => {
        return data['ListLastStatusDevice'];
      }));
  }

  getGeocercasAsignacion() {
    return this.serviceClass.getQuery(`asignacion/geo/fence`)
      .pipe( map(data => {
        return data['fence']['GeoFence'];
      }));
  }

  setNuevaGeocerca(data: any) {
    return this.serviceClass.postQuery(`geo/fence`, data);
  }

  setUpdateGeocerca(data: any) {
    return this.serviceClass.putQuery(`geo/fence`, data);
  }

  getGeocerca(id: string) {
    return this.serviceClass.getQuery(`geo/fence?id=${id}`)
      .pipe( map(data => {
        return data['GeoFence'];
      }));
  }

  getListGeocercas() {
    return this.serviceClass.getQuery(`geo/fence/list`)
      .pipe( map(data => {
        return data['listGeoFence'];
      }));
  }

  setNuevaAsignacion(data: any) {
    return this.serviceClass.postQuery(`asignacion/geo/fence`, data);
  }

  
  setDeleteAsignacion(data: any) {
    return this.serviceClass.postQuery(`asignacion/geo/fence/device`, data);
  }

  setDeleteGeocerca(id: string) {
    return this.serviceClass.deleteQuery(`geo/fence?id=${id}`);
  }





}
