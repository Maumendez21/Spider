import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { ServiceClass } from '../../utils/services-class';



@Injectable({
  providedIn: 'root'
})
export class AuthService {


  public serviceClass = null;
  constructor(
    private http: HttpClient,
    private router: Router
  ) {

    this.serviceClass = new ServiceClass(this.http);
  }

  get token(): string {
    return this.serviceClass.token;
  }

  login(data:any) {
    return this.serviceClass.postQuery("access/login", data);
  }

  recoveryEmail(email: string){
    return this.serviceClass.getQuery(`send/email?email=${email}`);
  }








}
