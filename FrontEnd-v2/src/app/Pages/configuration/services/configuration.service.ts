import { Injectable } from '@angular/core';
import { ServiceClass } from '../../../utils/services-class';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ConfigurationService {

  public serviceClass = null;

  constructor(
    private http: HttpClient,
  ) {
    this.serviceClass = new ServiceClass(this.http);
  }

  getListSubempresas() {
    return this.serviceClass.getQuery("administration/subcompanies/list")
     .pipe( map(data => {
       return data['listSubCompany'];
     }));
  }

  getListResponsables(responsible: string = '') {
    return this.serviceClass.getQuery(`configuration/responsible/list?search=${responsible}`)
     .pipe( map(data => {
       return data['listResponsible'];
     }));
  }


  setNewSubempresa(data: any) {
    return this.serviceClass.postQuery("administration/subcompanies", data);
  }

  setUpdateSubempresa(data: any) {
    return this.serviceClass.putQuery("administration/subcompanies ", data);
  }

  

  

  /*Devices*/


  getListDevicesConfiguration() {
    return this.serviceClass.getQuery("obd/admin/configuration/list/obd")
      .pipe( map(data => {
        return data['listObd'];
      }));
  }

  setAssignmentSubempresa(data: any) {
    return this.serviceClass.putQuery("obd/admin/obd", data);
  }


  /* ALARMAS */

  getAlarmasAdmin(device: string, fechaInicio: string, fechafin: string){
    return this.serviceClass.getQuery(`administration/alarms/list?device=${device}&startdate=${fechaInicio}&enddate=${fechafin}`)
    .pipe( map(data => {
      return data['ListAlarms'];
    }));
  }

  /* RESPONSABLES */
  
  deleteResponsable(id: number) {
    return this.serviceClass.deleteQuery(`configuration/responsible?id=${id}`);
  }
  
  
  getResponsable(id: number) {
    return this.serviceClass.getQuery(`configuration/responsible?id=${id}`)
    .pipe( map(data => {
      return data['registry'];
    }));
  }
  
  setUpdateResponsable(data: any) {
    return this.serviceClass.putQuery("configuration/responsible", data);
  }
  
  setNuevoResponsable(data: any) {
    return this.serviceClass.postQuery("configuration/responsible", data);
  }
  
  


  

  

  

  




}
