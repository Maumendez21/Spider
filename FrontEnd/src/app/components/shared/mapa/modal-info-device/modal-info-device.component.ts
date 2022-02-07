import { Component, Input, OnInit } from '@angular/core';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import Swal from 'sweetalert2';
import { InfoDevice2 } from 'src/app/models/info-device/info-device2';
declare const google: any;

@Component({
  selector: 'app-modal-info-device',
  templateUrl: './modal-info-device.component.html',
  styleUrls: ['./modal-info-device.component.css']
})
export class ModalInfoDeviceComponent implements OnInit {

  @Input() device: any;
  @Input() tipo: number;
  @Input() infoDevice: any;
  @Input() alarmas: any;
  @Input() setDevice: any;
  @Input() getEstatusName: any;
  @Input() getTipoDispositivo: any;
  @Input() logotipo: any;
  @Input() paroMotor: any;
  @Input() infoDevice2: any = {};







  latitud: number = 19.4525976;
  longitud: number = -99.1182164;
  nombre: string;
  zoom: number = 11;
  map: any;
  fitBounds: boolean = true;
  agmFitBounds: boolean = true;


  // descriptionTipe: string;

  constructor(private spiderService: SpiderfleetService) {
    // this.descriptionTipe = this.infoDevice2.DescriptionType;
  }
  getParoMotorDevice(device: string){
    this.spiderService.getEngineDevice(device).subscribe(data => {
      this.paroMotor = data;
    })
  }

  onMapReady(map: any) {
    this.map = map;

  }

  role: string = localStorage.getItem('role');

  viewParodeMotor(): boolean{
    if (this.infoDevice.EngineStop === 1 && this.infoDevice.TypeDevice === 8 && this.role == "Administrador") {
      return true;
    }else {
      return false;
    }
  }

  viewDetalle2(): boolean {
    if (this.infoDevice.TypeDevice === 7 &&  5 && 6 && 4) {
      return true;
    }else {
      return false;
    }
  }

  ngOnInit(): void {
    // this.getParoMotorDevice(this.infoDevice.Device)
  }

  showMap(latitud: number, longitud: number, alarma: string ){
    this.latitud = latitud;
    this.longitud = longitud;
    this.nombre = alarma;
  }

  deshabilitado(status: number): boolean{
    if (status === 0) {
      return true
    }else if(status === 1) {
      return true
    }else if (status === 2) {
      return false
    }else if (status === 3) {
      return true
    }
  }
  deshabilitadoParo(status: number): boolean{
    if (status === 0) {
      return false
    }else if(status === 1) {
      return true
    }else if (status === 2) {
      return true
    }else if (status === 3) {
      return true
    }
  }

  paroMotorFUN(device: string, status: number, tipe: number){

    Swal.fire({
      title: tipe === 0 ? 'Paro de Motor': 'Activación de motor',
      text: tipe === 0 ? '¿Estas seguro que deseas realizar el paro de motor?': '¿Estas seguro que deseas activar el motor?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#ff6e40',
      cancelButtonColor: '#555555',
      cancelButtonText: 'Cancelar',
      confirmButtonText: 'Si',
      allowOutsideClick: false
    }).then((result) => {
      if (result.value) {
        this.spiderService.setStopEngine(device, status)
        .subscribe(resp => {
          if(resp['success']){
            Swal.fire({
              title: tipe === 0 ? 'Se envio tu solicitud para hacer el paro de motor': 'Se envio tu solicitud para hacer la activación de motor',
              icon: 'success',
              confirmButtonColor: '#ff6e40',
            })
            // this.getDevicesEngine('', this.pageActual);
            this.getParoMotorDevice(device);
          }else {
            Swal.fire(
              'Hubo un fallo',
              resp['messages'],
              'error',

            )
          }
        })
      }
    })
  }












}
