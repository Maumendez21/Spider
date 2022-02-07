import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ServiceClass } from '../../../utils/services-class';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MapService {

  public serviceClass = null;

  constructor(
    private http: HttpClient,
  ) {
    this.serviceClass = new ServiceClass(this.http);
  }

  getDevicesGeneralNew(device: string) {
    return this.serviceClass.getQuery(`main/last/position/devices?busqueda=${device}`)
      .pipe(
        map((data: any) => {
          return data;
        })
      );
  }

  getInfoDevice(device: string) {
    return this.serviceClass.getQuery(`vehicle/general/information/trip?device=${device}`)
      .pipe( map(data => {
        return data;
    }));
  }

  getInfoDevice2(device: string){
    return this.serviceClass.getQuery(`configuration/details?device=${device}`)
    .pipe( map(data => {
      return data['Registry'];
    }));
  }

  getEngineDevice(device: string){
    return this.serviceClass.getQuery(`engine/stop?device=${device}`)
      .pipe( map(data => {
        return data['EngineStop'];
      }));

  }
  setStopEngine(device: string, status: number){
    return this.serviceClass.postQuery(`engine/stop/execute?device=${device}&status=${status}`);
  }

  getLastTravelsDevice(device: string) {
    return this.serviceClass.getQuery(`vehicle/list/trips?device=${device}`)
      .pipe( map(data => {
        return data['listItineraries'];
      }));
  }

  getAllRoutesPerDate(device: string, startDate: string, endDate: string) {
    return this.serviceClass.getQuery(`vehicle/trips?device=${device}&startdate=${startDate}&enddate=${endDate}`)
      .pipe( map(data => {
        return data['listItineraries'];
      }));
  }

  getRouteDevice(device: string, startDate: string, endDate: string) {
    return this.serviceClass.getQuery(`vehicle/stroke?device=${device}&startdate=${startDate}&enddate=${endDate}`);
  }

  getNotifications() {
    return this.serviceClass.getQuery("main/last/position/notifications/priority");
  }

  getTripLink(device: string, startDate: string, endDate: string) {
    return this.serviceClass.getQuery(`vehicle/link?device=${device}&startdate=${startDate}&enddate=${endDate}`);
  }

  getActivityDay(device: string){
    return this.serviceClass.getQuery(`dashboard/activity/day/list/devices?busqueda=${device}`)
      .pipe( map(data => {
        return data;
      }));
  }

  setChangeStatusNotification(id: string) {
    return this.serviceClass.putQuery(`notifications/priority?id=${id}`);
  }

  getGraphicsDetails(device: string){
    return this.serviceClass.getQuery(`card/graphics/odo?device=${device}`);
  }



}
