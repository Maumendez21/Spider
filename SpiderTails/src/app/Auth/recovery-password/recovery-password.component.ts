import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-recovery-password',
  templateUrl: './recovery-password.component.html',
  styleUrls: ['./recovery-password.component.css']
})
export class RecoveryPasswordComponent implements OnInit {

  email: string = "";
  error: boolean = false;
  btnEnviar: any = {
    text: "Enviar",
    loading: false
  }

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
  }

  onSubmit(){
    if(this.email !== "")
    {
      this.btnEnviar = {
        text: "Enviando...",
        loading: true
      }
      this.authService.recoveryEmail(this.email).subscribe(resp => {
        if (resp['success']) {
          // this.toastr.success("Enviamos la nueva contraseña a tu correo", "Exito!");
          Swal.fire('Enviado!', 'Enviamos la nueva contraseña a tu correo', 'success')
          this.router.navigate(['/login']);
        }else {
          // this.toastr.error(resp['messages'], "Error!");
          
          Swal.fire('Error!', ''+resp['messages'], 'error')
          this.btnEnviar = {
            text: "Enviar",
            loading: false
          }
        }
      })
    }else{
      this.error = true;
      Swal.fire('Error!', 'Ingresa tu correo', 'error')
    }
  }

}
