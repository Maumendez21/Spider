import { Component, OnInit } from '@angular/core';
import { ConfigurationService } from '../services/configuration.service';
import { InfoVehicleService } from '../services/info-vehicle.service';

@Component({
  selector: 'app-info-vehicle',
  templateUrl: './info-vehicle.component.html',
  styleUrls: ['./info-vehicle.component.css']
})
export class InfoVehicleComponent implements OnInit {

  id: string;
  vehiculoInfo: any;
  pageOfItems: Array<any>;
  public searchValue: string = '';

  constructor(
    private configurationService: ConfigurationService,
    private infoVehicle: InfoVehicleService
  ) { 
    this.getVehiculoInfo(this.searchValue);
  }

  ngOnInit(): void {

  }

  getVehiculoInfo = (value : string = '') => {
    this.infoVehicle.getListInfoVehiculo(value).subscribe(response => {
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
