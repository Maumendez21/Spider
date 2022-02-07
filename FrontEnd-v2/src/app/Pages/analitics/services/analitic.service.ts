import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { ServiceClass } from '../../../utils/services-class';

@Injectable({
  providedIn: 'root'
})
export class AnaliticService {

  public serviceClass = null;

  constructor(
    private http: HttpClient,
  ) {
    this.serviceClass = new ServiceClass(this.http);
  }

  getActivityDay(device: string){
    return this.serviceClass.getQuery(`dashboard/activity/day/list/devices?busqueda=${device}`)
      .pipe( map(data => {
        return data;
      }));
  }

  getAnalitcs(fechaInicio: string, fechaFin: string, grupo: string, device: string) {
    return this.serviceClass.getQuery(`dashboard/distancia/litros/tiempo?fechainicio=${fechaInicio}&fechafin=${fechaFin}&grupo=${grupo}&device=${device}`);
  }

  getHeatMap(startDate: string, endDate: string, group: string, device: string) {
    return this.serviceClass.getQuery(`heat/map?startdate=${startDate}&enddate=${endDate}&group=${group}&device=${device}`)
      .pipe( map(data => {
        return data['Coords'];
      }));
  }

  getNotificatonsAnaliticas(fechaInicio: string, fechafin: string){
    return this.serviceClass.getQuery(`dashboard/activity/day/list/notifications?start=${fechaInicio}&end=${fechafin}`);
  }





}
