import { Component, OnInit } from '@angular/core';
import { SharedService } from 'src/app/Services/shared.service';
import { NavigationEnd, Router } from '@angular/router';
import { filter, map } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { MapService } from 'src/app/Pages/home/Services/map.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  public name: string;
  public subName: string;
  public vehiculo: string = "";
  public spiderMarkers: any = [];
  public estatus: string = "";
  public type: string = "";
  public subempresa: string = "";

  public route: string = "";

  permisos: string;

  routeSubs$: Subscription;
  public notify: Subscription;
  notifications: string;
  unidades: string;
  unidadesInactivo: string;
  unidadesTotal: string;

  constructor(
    private shared: SharedService,
    private router: Router,
    private mapService: MapService
  ) {
    this.verifyName();
    this.getDevices();
    this.getFilters();
    this.getActivityDat();
    this.preparePermisos()
    this.crearNombreRutaLogotipo();



    this.routeSubs$ = this.getRoute().subscribe(data => {
      this.route = data;
    })

    this.notify = this.shared.notificationsStream$.subscribe(data => {
      this.notifications = data.toString();
    })

  }

  loading: boolean = true;

  async getActivityDat(){
    this.loading = true;
    await this.mapService.getActivityDay("").subscribe(data => {
      this.unidades = data['Actives']
      this.unidadesInactivo = data['Inactives'];
      this.unidadesTotal = data['Total'];
      this.loading = false;
    })
  }

  preparePermisos() {
    this.shared.permisosStream$.subscribe(response => {
      // this.permisos = response;

    });

    if (!this.permisos) {
      this.permisos = localStorage.getItem('permits');
    }
    
  }



  getRoute(){
    return this.router.events
    .pipe(
      filter(event => event instanceof NavigationEnd),
      map((event: NavigationEnd)=> event.url)
    )
  }

  showMenu = (headerToggle, navbarId) =>{


    const toggleBtn = document.getElementById(headerToggle),
    nav = document.getElementById(navbarId)

    // Validate that variables exist
    if(headerToggle && navbarId){

      // We add the show-menu class to the div tag with the nav__menu class
      nav.classList.toggle('show-menu')
      // change icon
      toggleBtn.classList.toggle('bx-x')

    }
  }

  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    this.routeSubs$.unsubscribe();
    // this.notify.unsubscribe();
  }

  ngOnInit(): void {
  }

  signOut() {
    this.shared.signOut();
  }

  getDevices() {
    this.shared.spiderMarkersStream$.subscribe(data => {
      this.spiderMarkers = data;
    });
  }


  changeFilterVehiculo() {
    this.shared.broadcastFilterVehiculoStream({
      vehiculo: this.vehiculo,
      search: true
    });
  }

  rightMenu(){
    this.shared.broadcastRightMenuStream({
      title: 'Mapa',
      tipe: true
    })
  }

  verifyName() {

    this.subName = localStorage.getItem("role")

    this.shared.nameStream$.subscribe(data => {
      this.name = data;
    });

    if (this.name == "N/D") {
      this.shared.broadcastNameStream(localStorage.getItem("name"));
    }
  }

  selectDevice(device: string, latitud: string, longitud: string, estatus: number, index: number) {

    this.shared.broadcastZoomCoordsStream({
      device: device,
      estatus: estatus,
      latitud: latitud,
      longitud: longitud,
      zoom: 11,
      bottom: false,
      filterBottom: false,
      startDate: '',
      endDate: ''
    });

    document.getElementById("btnModalInfoDevice").click();
  }

  getFilters() {

    this.shared.filterSubempresaStream$.subscribe(data => {
      this.subempresa = data.subempresa;
    });

    this.shared.filterEstatusStream$.subscribe(data => {
      this.estatus = data.estatus;
    });
    this.shared.filterTypeStream$.subscribe(data => {
      this.type = data.typeV;
    });

    this.shared.filterVehiculoStream$.subscribe(data => {
      this.vehiculo = data.vehiculo;
    });
  }

  logoD: string;
  ruta = 'https://spiderfleetapi.azurewebsites.net/templates2/';

  crearNombreRutaLogotipo() {
    let idu = localStorage.getItem('idu');
    if(idu){
      let array = idu.split('-');
      this.logoD = this.ruta + array[0] + '.png?' + new Date().getTime();
      // this.logoD =  "https://spiderfleetapi.azurewebsites.net/templates2/85.png";
      //this.shared.broadcstLogoStream(this.logoD);
      
      

    }
  }

  errorHandler(event) {
    event.target.src = "https://spiderfleetapi.azurewebsites.net/templates2/0.png";
  }

}
