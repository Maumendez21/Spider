import { Injectable } from '@angular/core';
import { ServiceClass } from '../../../utils/services-class';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ParametersService {

  public serviceClass = null;

  constructor(
    private http: HttpClient,
  ) {
    this.serviceClass = new ServiceClass(this.http);
  }

  getListParametrosGenerales(){
    return this.serviceClass.getQuery('configuration/list')
    .pipe( map(data => {
      return data['ListResgistry'];
    }));
  }

  getParametro(id: number){
    return this.serviceClass.getQuery(`configuration?id=${id}`)
      .pipe( map(data => {
        return data['Registry'];
      }));
  }

  
  setParametro(data: any){
    return this.serviceClass.putQuery('configuration', data);
  }

}
