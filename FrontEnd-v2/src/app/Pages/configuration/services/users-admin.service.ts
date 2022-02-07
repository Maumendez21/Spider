import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ServiceClass } from '../../../utils/services-class';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UsersAdminService {

  public serviceClass = null;
  constructor(
    private http: HttpClient,
  ) {
    this.serviceClass = new ServiceClass(this.http);
  }

  /* USUARIOS */
  getListUsuariosConfiguracion(value: string = '') {
    return this.serviceClass.getQuery(`administration/subusers/list?search=${value}`)
      .pipe( map(data => {
        return data['listUsers'];
      }));
  }

  getUsuarioConfiguracion(id: number) {
    return this.serviceClass.getQuery(`administration/subusers?id=${id}`)
      .pipe( map(data => {
        return data['user'];
      }));
  }

  getRoles() {
    return this.serviceClass.getQuery('administration/roles/list/roles')
      .pipe( map(data => {
        return data['listRoles'];
      }));
  }

  getEstatus() {
    return this.serviceClass.getQuery('administration/status/list')
      .pipe( map(data => {
        return data['listStatus'];
      }));
  }

  setNewUsuarioConfiguracion(data: any) {
    return this.serviceClass.postQuery('administration/subusers', data);
  }

  setUpdateUsuarioConfiguracion(data: any) {
    return this.serviceClass.putQuery('administration/subusers', data);
  }

  getPermissions(user: string){
    return this.serviceClass.getQuery(`configuration/permission/list?user=${user}`)
      .pipe( map(data => {
        return data['modules'];
      }));
  }

  setPermission(data: any){
    return this.serviceClass.postQuery('configuration/permission', data);
  }
}
