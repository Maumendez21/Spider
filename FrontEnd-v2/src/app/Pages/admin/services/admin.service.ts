import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ServiceClass } from '../../../utils/services-class';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  public serviceClass = null;

  constructor(
    private http: HttpClient,
  ) {
    this.serviceClass = new ServiceClass(this.http);
  }

  /* USUARIOS */
  setUpdateUser(data: any) {
    return this.serviceClass.putQuery("administration/users", data);
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

  /* RUTAS */

  getRutasAdmin(company: string, device: string, fechaIni: string, fechaFin: string){

    return this.serviceClass.getQuery(`administration/raw/data/list?company=${company}&device=${device}&startdate=${fechaIni}&enddate=${fechaFin}`)
    .pipe( map(data => {
      return data['ListRawData'];
    }));
   }

   /* EMPRESAS */

   getCompanies() {
    return this.serviceClass.getQuery("administration/companies/list/company")
     .pipe( map(data => {
       return data['listCompany'];
     }));
  }

  getListCompanies() {
    return this.serviceClass.getQuery("administration/company/list")
     .pipe( map(data => {
       return data['ListCompany'];
     }));
  }

  setCompanysAccess(data: any){
    return this.serviceClass.putQuery("administration/company", data);
  }

   /* DISPOSITIVOS */
  
  getListDevicesAdministration() {
    return this.serviceClass.getQuery("obd/admin/manager/list/obd")
      .pipe( map(data => {
        return data['listObd'];
      }));
  }

  setNewDevice(data: any) {
    return this.serviceClass.postQuery("management/obd", data);
  }

  setUpdateDevice(data: any) {
    return this.serviceClass.putQuery("management/obd", data);
  }

  getDevice(id: string) {
    return this.serviceClass.getQuery(`management/obd?id=${id}`)
      .pipe( map(data => {
        return data['obd'];
      }));
  }

  /* CARGA MASIVA  */

   sendCargaMasiva(idEmpresa: string, formData: FormData) {
    return this.serviceClass.postQuery(`bulk/load/Obds?empresa=${idEmpresa}`, formData);
  }

  getListSims() {
    return this.serviceClass.getQuery("sim/available")
      .pipe( map(data => {
        return data['listSims'];
      }));
  }

}
