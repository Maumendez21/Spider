import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment.prod';
import { SharedService } from '../../Services/shared.service';

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

  ngOnInit(): void {
    this.crearNombreRutaLogotipo();
    this.preparePermisos();
    this.production = environment.production;
    
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
