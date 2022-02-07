import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';
import { PorfileService } from '../services/porfile.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {

  contraseniaActual: string = "";
  contrasenia1: string = "";
  contrasenia2: string = "";
  error: boolean = false;

  constructor(
    private porfileService: PorfileService
  ) { }

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

            this.porfileService.setContrasenia(data)
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
        // this.toastr.error('Las contraseñas no coinciden', 'ERROR!');
      }

    } else {
      this.error = true;
    }
  }

}
