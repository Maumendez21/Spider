import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-paro-de-motor',
  templateUrl: './paro-de-motor.component.html',
  styleUrls: ['./paro-de-motor.component.css']
})
export class ParoDeMotorComponent implements OnInit {

  devices : any[];
  pageActual: number = 1;
  options: any[] = [];
  name: string = '';

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService, private router: Router, private shared: SharedService) {
    this.limpiarFiltrosMapa();

    if (shared.verifyLoggin()) {
      this.getDevicesEngine('', 1);
      this.getPages('');
    } else {
      this.router.navigate(['/login']);
    }
  }

  changePage(page: number){
    this.pageActual = page;
    this.getDevicesEngine('', page);
  }

  buscarName(){
    this.getPages(this.name);
    this.getDevicesEngine(this.name, 1);

  }

  limpiarFiltrosMapa() {
    this.shared.limpiarFiltros();
    if (!document.getElementById("sidebarRight").classList.toggle("active")) {
      document.getElementById("sidebarRight").classList.toggle("active")
    }
  }

  getPages(name: string){
    this.spiderService.getPagesEngineStop(name)
    .subscribe((data: number) => {
      for (let index = 0; index < data  ; index++) {
        this.options[index] = index+1;
      }
    })
  }

  getDevicesEngine(name: string, page: number){
    this.spiderService.getListEngineStop(name, page)
    .subscribe(data=>{
      console.log(data);

      this.devices = data;
    })
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

  paroMotor(device: string, status: number, tipe: number){

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
          console.log(resp);

          if(resp['success']){
            Swal.fire({
              title: tipe === 0 ? 'Se envio tu solicitud para hacer el paro de motor': 'Se envio tu solicitud para hacer la activación de motor',
              icon: 'success',
              confirmButtonColor: '#ff6e40',
            })
            this.getDevicesEngine('', this.pageActual);
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



  ngOnInit(): void {
  }

}
