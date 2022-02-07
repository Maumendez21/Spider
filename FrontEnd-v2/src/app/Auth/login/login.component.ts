import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { LoginModel } from '../models/login.model';
import { SharedService } from 'src/app/Services/shared.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginModel: LoginModel;

  usuario: string;

  error: boolean = false;


  errorLogin: any = {
    show: false,
    text: ""
  };
  btnLoading: any = {
    text: "Ingresar",
    loading: false
  }

  constructor(
    private authService: AuthService,
    private shared: SharedService,
    private router: Router,
  ) {


    if (localStorage.getItem("token")) {
      this.shared.broadcastLoggedStream(true);
      this.router.navigate(['/']);
    } else {
      this.loginModel = new LoginModel();

    }
  }

  ngOnInit(): void {
  }

  onSubmit(){

    this.btnLoading.text = "Cargando...";

    if (this.loginModel.user != "" && this.loginModel.password != "") {

      const data:any = {
        'Username': this.loginModel.user,
        'Password': this.loginModel.password
      };

      this.authService.login(data)
        .subscribe(data => {

          if (data['messages'][0] == "Acceso correcto") {

            localStorage.setItem("token", data['access_token']);
            localStorage.setItem("tokenRefresh", data['refresh_token']);
            localStorage.setItem("expiresIn", data['expires_in']);
            localStorage.setItem("name", data['name']);
            localStorage.setItem("role", data['role']);
            localStorage.setItem("idu", data['idu']);
            localStorage.setItem("company", data['spider']);
            localStorage.setItem("permits", data['listPermissions']);

            this.shared.broadcastLoggedStream(true);
            this.shared.broadcastNameStream(data['name']);
            this.shared.broadcastLogoStream(true);
            // this.shared.broadcastPermisosStream(data['listPermissions']);

            if (localStorage.getItem("trip") !== null) {

              const tripRoute = localStorage.getItem("trip");
              localStorage.removeItem("trip");
              window.location.href = tripRoute;
            } else {
              this.router.navigate(['/home']);
            }

          } else {

            this.errorLogin.show = true;
            this.errorLogin.text = data['messages'][0];

            this.error = true;
            this.btnLoading.text = "Ingresar";
          }
        });

    } else {
      this.errorLogin.show = true;
      this.errorLogin.text = 'LLena todos los campos';
      this.error = true;
      this.btnLoading.text = "Ingresar";
      this.shared.broadcastLoggedStream(false);
    }

  }

}
