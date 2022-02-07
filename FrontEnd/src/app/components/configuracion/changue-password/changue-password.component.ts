import { Component, OnInit } from '@angular/core';
import { SpiderfleetService } from '../../../services/spiderfleet.service';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from '../../../services/shared.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-changue-password',
  templateUrl: './changue-password.component.html',
  styleUrls: ['./changue-password.component.css']
})
export class ChanguePasswordComponent implements OnInit {

  contraseniaActual: string = "";
  contrasenia1: string = "";
  contrasenia2: string = "";
  error: boolean = false;

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService, private shared: SharedService, private router: Router) {
    this.limpiarFiltrosMapa();
    if (this.shared.verifyLoggin()) {
    } else {
      this.router.navigate(['/login']);
    }

  }

  limpiarFiltrosMapa() {
    this.shared.limpiarFiltros();
    if (!document.getElementById("sidebarRight").classList.toggle("active")) {
      document.getElementById("sidebarRight").classList.toggle("active")
    }
  }

  ngOnInit(): void {
  }

  actualizarContrasenia(){
    if ( this.contraseniaActual !== "" || this.contrasenia1 !== "" || this.contrasenia2 !== "") {
      if(this.contrasenia1 === this.contrasenia2){
        Swal.fire({
          title: 'Cambio de contraseña',
          text: '¿Estas seguro que deseas actualizar la contraseña?',
          icon: 'warning',
          showCancelButton: true,
          confirmButtonColor: '#ff6e40',
          cancelButtonColor: '#555555',
          cancelButtonText: 'Cancelar',
          confirmButtonText: 'Si, actualizar',
          allowOutsideClick: false
        }).then((result) => {
          if (result.value) {
            const data = {
              Login: "",
              Password: this.contrasenia1,
              OldPassword: this.contraseniaActual
            };

            this.spiderService.setContrasenia(data)
            .subscribe(resp => {
              if(resp['success']){
                this.contrasenia1 = '';
                this.contraseniaActual = '';
                this.contrasenia2 = '';
                Swal.fire({
                  title: 'Contraseña actualizada',
                  text: 'La contraseña se actualizo correctamente.',
                  icon: 'success',
                  confirmButtonColor: '#ff6e40',
                })
              }else {
                Swal.fire(
                  'Hubo un fallo',
                  '' + resp['messages'],
                  'error',

                )
              }
            })
          }
        })

      }else {
        this.toastr.error('Las contraseñas no coinciden', 'ERROR!');
      }

    } else {
      this.error = true;
    }
  }

}
