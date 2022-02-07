import { Component, Input, OnInit } from '@angular/core';
import Swal from 'sweetalert2';
import { MapService } from '../../../Services/map.service';

@Component({
  selector: 'app-paro-motor-tab',
  templateUrl: './paro-motor-tab.component.html',
  styleUrls: ['./paro-motor-tab.component.css']
})
export class ParoMotorTabComponent implements OnInit {

  constructor(
    private mapService: MapService,
  ) { }

  @Input() paroMotor: any;

  ngOnInit(): void {
  }

  ngOnChanges(): void {
    //Called before any other lifecycle hook. Use it to inject dependencies, but avoid any serious work here.
    //Add '${implements OnChanges}' to the class.
    
  }

  getParoMotorDevice(device: string){
    this.mapService.getEngineDevice(device).subscribe(data => {
      console.log(data);
      
      this.paroMotor = data;
    })
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
        this.mapService.setStopEngine(device, status)
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
