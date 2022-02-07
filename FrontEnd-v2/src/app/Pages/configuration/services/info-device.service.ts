import { Injectable } from '@angular/core';
import { ServiceClass } from '../../../utils/services-class';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class InfoDeviceService {

  public serviceClass = null;

  constructor(
    private http: HttpClient,
  ) {
    this.serviceClass = new ServiceClass(this.http);
  }


  getListDispositivosInfo(device: string = ''){
    return this.serviceClass.getQuery(`configuration/details/list?search=${device}`)
      .pipe( map(data => {
        return data['ListDetails'];
      }));
  }

  getInfoDispositivioId(id: string){
      return this.serviceClass.getQuery(`configuration/details?device=${id}`)
        .pipe( map(data => {
          return data['Registry'];
        }));
  }

  getListRedInfoDevice(){
    return this.serviceClass.getQuery(`configuration/communication/methods/list`)
      .pipe( map(data => {
        return data['ListComMethods'];
      }));
  }

  getListTimeInfoDevice(){
    return this.serviceClass.getQuery(`configuration/sampling/time/list`)
      .pipe( map(data => {
        return data['ListSamTime'];
      }));
  }

  postInfoDevice(data: any) {
    return this.serviceClass.postQuery(`configuration/details`, data);
  }

  getTypeDevices() {
    return this.serviceClass.getQuery("inventario/typedevices/list")
      .pipe( map(data => {
        return data['listTypeDevice'];
      }));
  }

}
