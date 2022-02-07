import { Injectable } from '@angular/core';
import { ServiceClass } from '../../../utils/services-class';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PorfileService {

  public serviceClass = null;

  constructor(
    private http: HttpClient,
  ) {
    this.serviceClass = new ServiceClass(this.http);
  }

  setContrasenia(data: any){
    return this.serviceClass.putQuery('password/changepassword', data);
  }

  setLogo(data: FormData){
    return this.serviceClass.postQuery('changed/logo', data);
  }

  updateLogo(data: FormData){
    return this.serviceClass.postQuery('changed/logo/image/update', data);
  }


}
