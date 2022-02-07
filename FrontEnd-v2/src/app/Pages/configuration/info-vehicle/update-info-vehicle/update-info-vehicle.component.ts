import { Component, OnInit, Input } from '@angular/core';
import Swal from 'sweetalert2';
import { ConfigurationService } from '../../services/configuration.service';
import { InfoVehicleService } from '../../services/info-vehicle.service';

@Component({
  selector: 'app-update-info-vehicle',
  templateUrl: './update-info-vehicle.component.html',
  styleUrls: ['./update-info-vehicle.component.css']
})
export class UpdateInfoVehicleComponent implements OnInit {

  @Input() idSend: string;
  @Input() reloadTable: any;

  markList: any;
  modelList: any;
  versionList: any;
  tipoList: any;

  device: string;
  name: string;
  placas: string;
  VIN: string;
  marca: string;
  poliza: string;
  modelo: string;
  version: string;
  tipo: string;

  constructor(
    private configurationService: ConfigurationService,
    private infoVehicle: InfoVehicleService
  ) { 
    this.getMarks();
    this.getModels();
    this.getTipoVehiculo();
  }

  ngOnChanges(): void {
    // console.log(this.idSend);
    this.getInfoVehiculo(this.idSend);
  }

  getInfoVehiculo(id: string){
    this.infoVehicle.getInfoVehiculo(id)
    .subscribe(data => {
      this.device = data.Device;
      this.name = data.Name;
      this.placas = data.Placas;
      this.VIN = data.VIN;
      this.poliza = data.Poliza;
      this.marca = data.IdMarca;


      if (data.IdMarca != null){
          this.getModels();
          this.modelo = data.IdModelo;
      }
      if (data.IdModelo != null){
          this.getVersion();
          this.version = data.IdVersion;
      }

      this.tipo = data.IdTipoVehiculo;
    });

  }

  ngOnInit(): void {
  }

  getMarks(){
    this.infoVehicle.getMarcaVehiculo()
    .subscribe(data => {
      this.markList = data;
    });
  }
  getModels(){
    this.infoVehicle.getModeloVehiculo(this.marca)
    .subscribe(data => {
      this.modelList = data;
    });
  }
  getVersion(){
    this.infoVehicle.getVersionVehiculo(this.modelo)
    .subscribe(data => {
      this.versionList = data;
    });
  }

  getTipoVehiculo(){
    this.infoVehicle.getTipoVehiculo()
    .subscribe(data => {
      this.tipoList = data;
    });
  }

  actualizarInfoVehiculo(){
    const data = {
      device: this.device,
      nombre: this.name,
      idMarca: this.marca,
      idModelo: this.modelo,
      idVersion: this.version,
      VIN: this.VIN,
      placas: this.placas,
      poliza: this.poliza,
      idTipoVehiculo: this.tipo
    };

    this.infoVehicle.setUpdateVehiculoInfo(data)
    .subscribe(response => {
      if(response['success']){
        this.reloadTable();
        Swal.fire('Actualizado!', 'La información se actualizó', 'success')
      }else{
        Swal.fire('ERROR!', '' + response['messages'] , 'error')
      }
    });
  }

}
