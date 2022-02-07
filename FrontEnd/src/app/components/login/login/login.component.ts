import { Component, OnInit } from '@angular/core';

import { LoginModel } from 'src/app/models/login/login.model';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginModel: LoginModel;
  error: boolean = false;
  errorLogin: any = {
    show: false,
    text: ""
  };
  btnLoading: any = {
    text: "Ingresar",
    loading: false
  }

  constructor(private shared: SharedService, private service: SpiderfleetService, private route: Router) {
    if (localStorage.getItem("token")) {
      this.shared.broadcastLoggedStream(true);
      this.route.navigate(['/mapa']);
    } else {
      this.loginModel = new LoginModel();
      //this.toggleSidebarRight();
    }
  }

  ngOnInit(): void {
  }

  toggleSidebarRight() {

    if (screen.width <= 768) {
      const sidebar = document.getElementById("sidebarRight");

      if (sidebar.classList.contains("active")) {
        sidebar.classList.toggle("active");
      }
    }
  }

  onSubmit() {

    this.btnLoading.text = "Cargando...";

    if (this.loginModel.user != "" && this.loginModel.password != "") {

      const data:any = {
        'Username': this.loginModel.user,
        'Password': this.loginModel.password
      };

      this.service.login(data)
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
            this.shared.broadcastPermisosStream(data['listPermissions']);

            if (localStorage.getItem("trip") !== null) {

              const tripRoute = localStorage.getItem("trip");
              localStorage.removeItem("trip");
              window.location.href = tripRoute;
            } else {
              this.route.navigate(['/mapa']);
            }

          } else {

            this.errorLogin.show = true;
            this.errorLogin.text = data['messages'][0];

            this.error = true;
            this.btnLoading.text = "Entrar";
          }
        });

    } else {
      this.error = true;
      this.btnLoading.text = "Entrar";
      this.shared.broadcastLoggedStream(false);
    }
  }

}
