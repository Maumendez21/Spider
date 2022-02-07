import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { ServiceClass } from '../utils/services-class';

@Injectable({
  providedIn: 'root'
})
export class SpiderService {

  public serviceClass = null;

  constructor(
    private http: HttpClient,
  ) {
    this.serviceClass = new ServiceClass(this.http);
  }

  getSubempresas() {
    return this.serviceClass.getQuery("main/subempresas")
      .pipe( map(data => {
        return data['listSubEmpresas'];
      }));
  }

  getDevicesGeneralNew(device: string) {
    return this.serviceClass.getQuery(`main/last/position/devices?busqueda=${device}`)
      .pipe( map((data: any) => {
        return data;
      }));
  }

  getDevicesAdmin(compani: string) {
    return this.serviceClass.getQuery(`administration/alarms/devices?company=${compani}`)
    .pipe( map(data => {
      return data['ListDevices'];
    }));
   }


}
