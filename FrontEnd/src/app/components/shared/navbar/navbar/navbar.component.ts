import { Component, OnDestroy, OnInit } from '@angular/core';
import { SharedService } from 'src/app/services/shared.service';
import { Router } from '@angular/router';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import { AppComponent } from 'src/app/app.component';
import { interval, Subscription, timer } from "rxjs";
import * as moment from 'moment';
moment.locale('es');

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit, OnDestroy {

  ruta = 'https://spiderfleetapi.azurewebsites.net/templates/logo/';
  logoD: string;
  logoMosrar: string;
  show: boolean;
  name: string;
  logo: string;
  notifications: string;
  fecha = '';

  public notify: Subscription;


  constructor(private app: AppComponent, private shared: SharedService, private route: Router, private spiderService: SpiderfleetService) {
    this.showNavbar();
    this.verifyName();
    this.verifyLogo();
    this.fechaInicio();
    // this.notifications = '2';

    this.notify = this.shared.notificationsStream$.subscribe(data => {
      this.notifications = data.toString();
    })


  }
  ngOnDestroy(): void {
    this.notify.unsubscribe();
  }



  fechaInicio(){
    const fecha = interval(1000);
    fecha.subscribe((n) => {
      const hoy = moment();

      this.fecha = hoy.format(`dddd D MMMM YYYY hh:mm a`);
    })
  }

  showNavbar() {
    this.shared.loggedStream$.subscribe(data => {
      this.show = data;
      this.logo = localStorage.getItem("company");
      this.compatyTitleBrowser();

      // if (data) {
      //   this.getNotifications();
      //   setInterval(() => {
      //     this.getNotifications();
      //   }, 30000);
      // }







    });

    if (!this.show) {
      this.shared.broadcastLoggedStream((localStorage.getItem("token") ? true : false ));
    }

  }

  crearNombreRutaLogotipo() {
    let idu = localStorage.getItem('idu');
    if(idu){
      let array = idu.split('-');
      this.logoD = this.ruta + array[0] + '.png?' + new Date().getTime();
      //this.shared.broadcstLogoStream(this.logoD);

    }
  }

  errorHandler(event) {
    event.target.src = "https://spiderfleetapi.azurewebsites.net/templates/logo/0.png";
  }

  ngOnInit(): void {
    this.crearNombreRutaLogotipo();

  }

  // async getNotifications() {
  //   await this.spiderService.getCountNotifications()
  //     .subscribe(data => {
  //       console.log(data);

  //       this.notifications = data['View'];
  //     });
  // }

  compatyTitleBrowser() {

    switch(this.logo) {
      case "0":
        this.app.setTitle("Fleet Managment");
        break;
      case "1":
        this.app.setTitle("Kernel Logistics");
        break;
      case "2":
        this.app.setTitle("Agua y Drenaje de Nuevo León");
        break;
      default:
        this.app.setTitle("Fleet Managment");
        break;
    }
  }

  verifyLogo(){
    this.shared.logoStream$.subscribe(data => {
      if (data) {
        this.crearNombreRutaLogotipo();
        this.shared.broadcastLogoStream(false);
      }
    })


  }

  verifyName() {

    this.shared.nameStream$.subscribe(data => {
      this.name = data;
    });

    if (this.name == "N/D") {
      this.shared.broadcastNameStream(localStorage.getItem("name"));
    }
  }

  signOut() {
    this.shared.signOut();
  }

  toggleMenu() {

    const textsSidebars = [
      "textSidebarOne",
      "textSidebarTwo",
      "textSidebarThree",
      "textSidebarFour",
      "textSidebarFive",
      "textSidebarSix",
      "textSidebarSeven",
      "textSidebarEight",
      "textSidebarNine",
      "textSidebarThen",
    ];

    const sidebar = document.getElementById("sidebar");

    sidebar.classList.toggle("active");

    if (sidebar.classList.contains("active")) {
      textsSidebars.forEach(x => {
        if (document.getElementById(x)) {
          document.getElementById(x).style.display = "none";
        }
      });

    } else {

      textsSidebars.forEach(x => {
        if (document.getElementById(x)) {
          document.getElementById(x).style.display = "inline";
        }
      });

    }
  }

}
