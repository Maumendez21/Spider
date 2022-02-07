import { Component, OnInit } from '@angular/core';
import { SharedService } from 'src/app/services/shared.service';

@Component({
  selector: 'app-mapa',
  templateUrl: './mapa.component.html',
  styleUrls: ['./mapa.component.css']
})
export class MapaComponent implements OnInit {

  estatus: string = "";
  class: boolean = false;
  public idu = "";

  constructor(private shared: SharedService) {
    shared.limpiarFiltros();
    this.updateColorEstatus();
    this.getActiveBtn();
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

  ngOnInit(): void {}

  updateColorEstatus() {
    this.shared.filterEstatusStream$.subscribe(data => {
      this.estatus = data;
    });
  }

  getActiveBtn(){
    this.shared.filterTypeStream$.subscribe(data => {
      this.class = data.search;
    });
  }

  limpiarFiltros() {
    document.getElementById("clear").click();
    this.shared.broadcastFilterSubempresaStream({
      subempresa: "",
      search: false
    });
    this.shared.broadcastFilterVehiculoStream({
      vehiculo: "",
      search: false
    });
    this.shared.broadcastFilterEstatusStream({
      estatus: "",
      search: false
    });
    this.shared.broadcastFiltertypeStream({
      typeV: "",
      search: false
    });
    this.shared.broadcastRouteDirectionStream([]);
    this.shared.broadcastZoomCoordsStream({
      device: "",
      latitud: '',
      longitud: '',
      zoom: 11,
      bottom: false,
      filterBottom: false,
      startDate: '',
      endDate: ''
    });
    this.shared.clusterDinamicoStream(true);
  }

  sidebarRight() {
    const sidebar = document.getElementById("sidebarRight");
    sidebar.classList.toggle("active");
  }

}
