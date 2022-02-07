import { Component, OnInit } from '@angular/core';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';

@Component({
  selector: 'app-sidebar-right-filters-mapa',
  templateUrl: './sidebar-right-filters-mapa.component.html',
  styleUrls: ['./sidebar-right-filters-mapa.component.css']
})
export class SidebarRightFiltersMapaComponent implements OnInit {

  show: boolean;
  spiderMarkers: any = [];
  subempresas: any = [];

  subempresa: string = "";
  vehiculo: string = "";
  estatus: string = "";
  type: string = "";

  statusCluster: boolean = true;
  statusMapColor: boolean = true;
  statusSatelite: boolean = true;
  statustrafficLayer: boolean = true;

  constructor(private shared: SharedService, private spider: SpiderfleetService) {

    this.shared.loggedStream$.subscribe(data => {
      this.show = data;

      if (shared.verifyLoggin()) {
        this.getDevices();
        this.getFilters();
        this.getSubempresas();
      } else {
        this.shared.clusterDinamicoStream(false);
        this.statusCluster = false;
        this.statusMapColor = false;
        this.statusSatelite = false;
        this.statustrafficLayer = false;
      }
    });
  }


  ngOnInit(): void {
  }

  getFilters() {

    this.shared.clusterDinamicoStream$.subscribe(data => {
      this.statusCluster = data;
    });

    this.shared.mapaDinamicoStream$.subscribe(data => {
      this.statusMapColor = data;
    });
    this.shared.mapaSateliteStream$.subscribe(data => {
      this.statusSatelite = data;
    });
    this.shared.trafficLayer$.subscribe(data => {
      this.statustrafficLayer = data;
    });

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

  getDevices() {
    this.shared.spiderMarkersStream$.subscribe(data => {
      this.spiderMarkers = data;
    });
  }

  getSubempresas() {
    this.spider.getSubempresas()
      .subscribe(data => {
        this.subempresas = data;
      });
  }

  changeFilterSubempresa() {
    this.shared.broadcastFilterSubempresaStream({
      subempresa: this.subempresa,
      search: true
    });
  }

  changeFilterVehiculo() {
    this.shared.broadcastFilterVehiculoStream({
      vehiculo: this.vehiculo,
      search: true
    });
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

  closeSidebarRight() {
    const sidebar = document.getElementById("wrapper");
    sidebar.classList.toggle("active");
  }

  changeViewCluster(e){
    this.shared.clusterDinamicoStream(e);
  }
  changeMapColor(e){
    this.shared.mapaDinamicoStream(e);
  }

  changeSatelit(e){
    this.shared.mapaSateliteStream(e);
  }
  changetrafficLayer(e){
    this.shared.trafficLayerStream(e);
  }

}
