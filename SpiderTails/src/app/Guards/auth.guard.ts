import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanLoad, Route, Router, RouterStateSnapshot, UrlSegment, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../Auth/services/auth.service';
// import { JwtHelperService } from '@auth0/angular-jwt'
import { JwtService } from '../Auth/services/jwt.service';
import { SharedService } from '../Services/shared.service';

// const helper = new JwtHelperService();

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate, CanLoad {

  constructor(
    private router: Router,
    private authService: AuthService,
    public jw: JwtService,
    public shared: SharedService
  ){

  }


  canLoad(route: Route, segments: UrlSegment[]): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    let salida = false;
    if (this.authService.token === null) {
      this.router.navigateByUrl('/login');
      salida = false;
    }else {
      salida = true;
    }
    return salida;
  }


  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot){
    let salida = false;
    if (this.authService.token === null) {
      this.router.navigateByUrl('/login');
      this.shared.signOut();
      salida = false;
    }else {

      salida = true;
    }
    return salida;
  }

  // public isAuthenticated(): boolean {

  //   const token = localStorage.getItem("token");
  //   let salida = !helper.isTokenExpired(token);
  //   console.log(salida);

  //   return salida;

  //   // Check whether the token is expired and return
  //   // true or false

  //   // return !this.jwtHelper.isTokenExpired(token);
  // }

}
