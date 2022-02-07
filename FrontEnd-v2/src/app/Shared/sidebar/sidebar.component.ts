import { Component, OnInit } from '@angular/core';
import { SharedService } from 'src/app/Services/shared.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {

  constructor(
    private shared: SharedService
  ) { }

  ruta = 'https://spiderfleetapi.azurewebsites.net/templates2/';
  logoD: string;
  permisos: string;
  production: boolean;
  idu: string = "";

  ngOnInit(): void {
    this.crearNombreRutaLogotipo();
    this.preparePermisos();
    this.production = environment.production;
    this.idu = localStorage.getItem("idu");
    
  }

  checkIdu(): boolean{
    switch (this.idu) {
      case '73':
        return false
      case '80':
        return false
      case '81':
        return false
      case '81-10':
        return false
      default:
        return true
    }
  }
  signOut() {
    this.shared.signOut();
  }

  click(){
    document.getElementById('btn-report').click();
  }


  crearNombreRutaLogotipo() {
    let idu = localStorage.getItem('idu');
    if(idu){
      let array = idu.split('-');
      this.logoD = this.ruta + array[0] + '.png?' + new Date().getTime();
      // this.logoD =  "https://spiderfleetapi.azurewebsites.net/templates2/85.png";
      //this.shared.broadcstLogoStream(this.logoD);
      // console.log(this.logoD);
      
      

    }
  }

  errorHandler(event) {
    event.target.src = "https://spiderfleetapi.azurewebsites.net/templates2/iconSpider.png";
  }

  preparePermisos() {
    

    if (!this.permisos) {
      this.permisos = localStorage.getItem('permits');
      // console.log(this.permisos.includes('MAP101'));
      
    }
  }

  setPermit(permit: string){
    this.shared.broadcastPermisosStream(permit);
  }


}
