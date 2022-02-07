import { Component, OnInit } from '@angular/core';
import { SpiderfleetService } from '../../../services/spiderfleet.service';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-vehiculo-info',
  templateUrl: './vehiculo-info.component.html',
  styleUrls: ['./vehiculo-info.component.css']
})
export class VehiculoInfoComponent implements OnInit {
  id: string;
  vehiculoInfo: any;
  pageOfItems: Array<any>;
  public searchValue: string = '';
  constructor(private spiderService: SpiderfleetService) {

    this.getVehiculoInfo(this.searchValue);
  }

  ngOnInit(): void {
  }

  getVehiculoInfo = (value : string = '') => {
    this.spiderService.getListInfoVehiculo(value).subscribe(response => {
      this.vehiculoInfo = response.map((x, i) => ({Device: x.Device, name: x.Name, marca: x.Marca, modelo: x.Modelo, version: x.Version, VIN: x.VIN, placas: x.Placas, poliza: x.Poliza, tipo: x.TipoVehiculo}));
    });
  }

  getIdDevice(id: string){
    this.id = id;
    // console.log(this.id);
  }

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }

}
