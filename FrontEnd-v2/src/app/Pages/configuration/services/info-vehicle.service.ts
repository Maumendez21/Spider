import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ServiceClass } from '../../../utils/services-class';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class InfoVehicleService {

  public serviceClass = null;

  constructor(
    private http: HttpClient,
  ) {
    this.serviceClass = new ServiceClass(this.http);
  }


  getListInfoVehiculo(value: string = ''){
    return this.serviceClass.getQuery(`administration/addtional/data/list?search=${value}`)
      .pipe( map(data => {
        return data['ListAddtional'];
      }));
  }

  
  
  getMarcaVehiculo(){
    return this.serviceClass.getQuery('administration/trade/marks/list')
    .pipe(map(data => {
      return data['ListMarks'];
    }));
  }

  getModeloVehiculo(idMarca: string){
    return this.serviceClass.getQuery(`administration/models/list?mark=${idMarca}`)
    .pipe(map(data => {
      return data['ListModels'];
    }));
  }

  getVersionVehiculo(idModelo: string){
    return this.serviceClass.getQuery(`administration/versions/list?model=${idModelo}`)
    .pipe(map(data => {
      return data['ListVersions'];
    }));
  }
  getTipoVehiculo(){
    return this.serviceClass.getQuery('administration/type/vehicles/list')
    .pipe(map(data => {
      return data['ListTypeVehicles'];
    }));
  }

  setUpdateVehiculoInfo(data: any){
    return this.serviceClass.postQuery('administration/addtional/data', data);
  }

  getInfoVehiculo(id: string) {
    return this.serviceClass.getQuery(`administration/addtional/data?device=${id}`)
      .pipe( map(data => {
        return data['addtional'];
      }));
  }

  setNewVersion(data: any) {
    return this.serviceClass.postQuery('administration/versions', data);
  }
}
