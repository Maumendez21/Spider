import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SpiderfleetService } from '../../../services/spiderfleet.service';

@Component({
  selector: 'app-formulario-recuperacion',
  templateUrl: './formulario-recuperacion.component.html',
  styleUrls: ['./formulario-recuperacion.component.css']
})
export class FormularioRecuperacionComponent implements OnInit {
  email: string = "";
  error: boolean = false;
  btnEnviar: any = {
    text: "Enviar",
    loading: false
  }
  constructor(private route: Router,  private toastr: ToastrService, private service: SpiderfleetService) { }

  ngOnInit(): void {
  }

  onSubmit(){
    if(this.email !== "")
    {
      this.btnEnviar = {
        text: "Enviando...",
        loading: true
      }
      this.service.recoveryEmail(this.email).subscribe(resp => {
        if (resp['success']) {
          this.toastr.success("Enviamos la nueva contraseña a tu correo", "Exito!");
          this.route.navigate(['/login']);
        }else {
          this.toastr.error(resp['messages'], "Error!");
          this.btnEnviar = {
            text: "Enviar",
            loading: false
          }
        }
      })
    }else{
      this.error = true;
    }
  }

}
