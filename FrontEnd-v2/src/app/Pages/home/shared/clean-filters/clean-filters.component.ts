import { Component, OnInit } from '@angular/core';
import { SharedService } from 'src/app/Services/shared.service';

@Component({
  selector: 'app-clean-filters',
  templateUrl: './clean-filters.component.html',
  styleUrls: ['./clean-filters.component.css']
})
export class CleanFiltersComponent implements OnInit {

  constructor(
    private shared: SharedService
  ) { }

  ngOnInit(): void {
  }

  limpiarFiltros() {
    document.getElementById("clear").click();
    this.shared.broadcastRightMenuStream({
      title: 'Mapa',
      tipe: true
    })
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

}
