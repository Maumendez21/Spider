import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { SharedService } from '../Services/shared.service';

@Injectable({
  providedIn: 'root'
})
export class PermitsGuard implements CanActivate {

  permisos: string;
  permit: string;

  constructor(private shared: SharedService, private router: Router){
    this.shared.permisosStream$.subscribe(response => {
       
      this.permit = response;
    });
    this.permisos = localStorage.getItem('permits');
  }


  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot) {
    console.log(this.permit); 
    console.log(this.permisos.includes(this.permit));

      if (this.permit !== '') {
        
        console.log('tiene permiso');
        // return this.permisos.includes(this.permit);
      }else {
        
        console.log('No tiene permiso');
        // this.router.navigateByUrl('/home');
      }
      
      return this.permisos.includes(this.permit);

  }
  
}
