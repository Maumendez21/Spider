import { Injectable } from '@angular/core';
import { ServiceClass } from '../../../utils/services-class';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MobilityService {

  public serviceClass = null;

  constructor(
    private http: HttpClient,
  ) {
    this.serviceClass = new ServiceClass(this.http);
  }

  getListRutasConfiguracion() {
    return this.serviceClass.getQuery('routes/list')
      .pipe( map(data => {
        return data['routes'];
      }));
  }

  postImportKml(formData: FormData){
    return this.serviceClass.postQuery('bulk/load/routes', formData);
  }

  setNuevaRutaConfiguracion(data: any) {
    return this.serviceClass.postQuery('routes', data);
  }

  getRuta(id: string) {
    return this.serviceClass.getQuery(`routes?id=${id}`)
      .pipe( map(data => {
        return data['routes'];
      }));
  }

  updateRuta(data: any) {
    return this.serviceClass.putQuery('routes', data);
  }

  getResponsablesMovilidad(device: string, fechaInicio: string, fechafin: string){
    return this.serviceClass.getQuery(`diary/responsibles/list?device=${device}&start=${fechaInicio}&end=${fechafin}`)
    .pipe( map(data => {
      return data['ListResponsibles'];
    }));
  }

  getPuntosInteresMovilidad(){
    return this.serviceClass.getQuery(`point/interest/list/service`)
    .pipe( map(data => {
      return data['ListPoints'];
    }));

  }
  getAnalisisPuntosInteresMovilidad(punto: string, fechaInicio: string, fechafin: string, device: string){
    return this.serviceClass.getQuery(`point/interest/analysis/list?mongo=${punto}&start=${fechaInicio}&end=${fechafin}&device=${device}`)
    .pipe( map(data => {
      return data;
    }));
  }


  getListEventosAgenda(startDate: string, endDate: string) {
    return this.serviceClass.getQuery(`configuration/diary/list?startdate=${startDate}&enddate=${endDate}`)
     .pipe( map(data => {
       return data['ListEvents'];
     }));
  }

  getListEventsRoutesAgenda(start: string, end: string) {
    return this.serviceClass.getQuery(`configuration/routes/diary/list?startdate=${start}&enddate=${end}`)
      .pipe( map(data => {
        return data['ListEvents'];
      }));
  }


  getListPuntosInteresDispositivos() {
    return this.serviceClass.getQuery("assignment/point/interest")
      .pipe( map(data => {
        return data['PointInterest']['PointInterest'];
      }));
  }

  
  deletePuntoInteres(id: string) {
    return this.serviceClass.deleteQuery(`point/interest?id=${id}`);
  }

  getPuntoInteres(id: string) {
    return this.serviceClass.getQuery(`point/interest?id=${id}`)
      .pipe( map(data => {
        return data['PointInterest'];
      }));
  }

  updatePuntoInteres(data: any) {
    return this.serviceClass.putQuery('point/interest', data);
  }

  
  setNuevoPuntoInteres(data: any) {
    return this.serviceClass.postQuery('point/interest', data);
  }

  postImportXSLX(formData: FormData){
    return this.serviceClass.postQuery('bulk/load/point/interest', formData);
  }

  getListPuntosInteres() {
    return this.serviceClass.getQuery('point/interest/list')
      .pipe( map(data => {
        return data['ListPointInterest'];
      }));
  }

  getDevicesGeneral(tipo:string, device:string) {
    return this.serviceClass.getQuery(`main/last/position/list/device?tipo=${tipo}&valor=${localStorage.getItem("idu")}&busqueda=${device}`)
      .pipe( map(data => {
        return data['ListLastPosition'];
      }));
  }

  setVincularPuntoDispositivos(data: any) {
    return this.serviceClass.postQuery(`assignment/point/interest`, data);
  }

  setDesvincularDispositivosPunto(data:any) {
    return this.serviceClass.postQuery(`assignment/point/interest/device`, data);
  }




}
