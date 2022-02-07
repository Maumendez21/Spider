import { Component, OnInit, Input } from '@angular/core';
import { SpiderfleetService } from '../../../../../services/spiderfleet.service';
import { SharedService } from '../../../../../services/shared.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { data } from 'jquery';
import { version } from 'process';

@Component({
  selector: 'app-update-vehiculo-info',
  templateUrl: './update-vehiculo-info.component.html',
  styleUrls: ['./update-vehiculo-info.component.css']
})
export class UpdateVehiculoInfoComponent implements OnInit {

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

  constructor(private spiderService: SpiderfleetService, private shared: SharedService, private router: Router, private toastr: ToastrService) {
    if (this.shared.verifyLoggin()) {
      this.getMarks();
      this.getModels();
      this.getTipoVehiculo();
    } else {
      this.router.navigate(['/login']);
    }
   }

  ngOnInit(): void {

  }
  ngOnChanges(): void {
    // console.log(this.idSend);
    this.getInfoVehiculo(this.idSend);
  }

  getInfoVehiculo(id: string){
    this.spiderService.getInfoVehiculo(id)
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

  getMarks(){
    this.spiderService.getMarcaVehiculo()
    .subscribe(data => {
      this.markList = data;
    });
  }
  getModels(){
    this.spiderService.getModeloVehiculo(this.marca)
    .subscribe(data => {
      this.modelList = data;
    });
  }
  getVersion(){
    this.spiderService.getVersionVehiculo(this.modelo)
    .subscribe(data => {
      this.versionList = data;
    });
  }

  getTipoVehiculo(){
    this.spiderService.getTipoVehiculo()
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

    this.spiderService.setUpdateVehiculoInfo(data)
    .subscribe(response => {
      if(response['success']){
        this.reloadTable();
        this.toastr.success("Exito al actualizar la información del vehiculo.");
      }else{
        this.toastr.error(response['messages'], "Error!");
      }
    });
  }


}
