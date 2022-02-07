import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ServiceClass } from '../../../utils/services-class';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MaitananceService {

  public serviceClass = null;

  constructor(
    private http: HttpClient,
  ) {
    this.serviceClass = new ServiceClass(this.http);
  }

  getInspeccionList(){
    return this.serviceClass.getQuery(`administration/inspection/list`)
    .pipe( map(data => {
      return data['InspectionList'];
    }));
   }

   getMechanics() {
    return this.serviceClass.getQuery(`administration/mechanics/list`)
    .pipe( map(data => {
      return data['ListMechanics'];
    }));
   }

   getPlantilla() {
    return this.serviceClass.getQuery(`administration/inspection/list/new`)
    .pipe( map(data => {
      return data['PlantillaHeader'];
    }));
 }

 getResponsableVehicle(device: string){
    return this.serviceClass.getQuery(`configuration/responsible/device?device=${device}`)
    .pipe( map(data => {
      return data['registry'];
    }));
  }

  postInspection(data: any){
    return this.serviceClass.postQuery("administration/inspection", data);
   }

   getInspecctionFolio(folio: string){
    return this.serviceClass.getQuery(`administration/inspection/list/folio?folio=${folio}`)
    .pipe( map(data => {
      return data['InspectionList'];
    }));
  }

  putInspeccion(data: any){
    return this.serviceClass.putQuery("administration/inspection", data);
   }



}
