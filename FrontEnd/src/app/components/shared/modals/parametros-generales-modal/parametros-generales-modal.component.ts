import { Component, OnInit, Input } from '@angular/core';
import { SpiderfleetService } from '../../../../services/spiderfleet.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from '../../../../services/shared.service';
import * as moment from 'moment';
moment.locale('es');

@Component({
  selector: 'app-parametros-generales-modal',
  templateUrl: './parametros-generales-modal.component.html',
  styleUrls: ['./parametros-generales-modal.component.css']
})
export class ParametrosGeneralesModalComponent implements OnInit {
  
  @Input() reloadTable: any;
  @Input()
  idSend: number;
  @Input()
  valor: any;
  @Input()
  desc: string;
  input: boolean;
  caracter: number;

  constructor(private spiderService: SpiderfleetService, private shared: SharedService, private router: Router, private toastr: ToastrService) { 
    
    if (this.shared.verifyLoggin()) {
      
    } else {
      this.router.navigate(['/login']);
    }
  }

  ngOnInit(): void {
  }

  ngOnChanges(): void {
    
    if (this.idSend) {
      this.getParameter(this.idSend);
      this.cambiarInput(this.valor); 
    }
  }

 
  getParameter(id: number){
    this.spiderService.getParametro(id)
    .subscribe(data => {
    })
  }
  
  
  cambiarInput(valor: any){
    let cadena = "" + valor;
    this.caracter = cadena.indexOf(':');
    if (this.caracter !== -1) {
      this.input = false;
    }else{
      this.input = true;
    }
  }
  
  
  actualizarParametro(){
    
    if (!this.input) {

      let hora = moment(this.valor, 'HH:mm:ss');
  
      if (hora.format('HH:mm:ss') !== 'Fecha inválida') {
        this.valor = hora.format('HH:mm:ss');
        const data = {
          Id: this.idSend,
          Value: this.valor
        }
        
        this.spiderService.setParametro(data)
        .subscribe(response => {
          if (response['success']) {
            this.toastr.success('Parametro actualizado correctamente', 'Exito!');
            this.reloadTable();
          }
          else{
            this.toastr.error(response['messages'], 'Error!')
          }
        }) 
      }
      else{
        this.toastr.error('Los valores no son correctos');
      }
      
    }else {
      const data = {
        Id: this.idSend,
        Value: this.valor
      }
      this.spiderService.setParametro(data)
        .subscribe(response => {
          if (response['success']) {
            this.toastr.success('Parametro actualizado correctamente', 'Exito!');
            this.reloadTable();
          }
          else{
            this.toastr.error(response['messages'], 'Error!')
          }
        }) 
    }    
  }  

}
