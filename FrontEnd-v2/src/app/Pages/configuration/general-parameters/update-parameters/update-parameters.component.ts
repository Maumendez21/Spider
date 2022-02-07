import { Component, OnInit, Input } from '@angular/core';
import { ConfigurationService } from '../../services/configuration.service';
import * as moment from 'moment';
import Swal from 'sweetalert2';
import { ParametersService } from '../../services/parameters.service';

@Component({
  selector: 'app-update-parameters',
  templateUrl: './update-parameters.component.html',
  styleUrls: ['./update-parameters.component.css']
})
export class UpdateParametersComponent implements OnInit {

  @Input() reloadTable: any;
  @Input()
  idSend: number;
  @Input()
  valor: any;
  @Input()
  desc: string;
  input: boolean;
  caracter: number;

  constructor(
    private parameterService: ParametersService
  ) { }

  ngOnInit(): void {
  }

  ngOnChanges(): void {
    
    if (this.idSend) {
      this.getParameter(this.idSend);
      this.cambiarInput(this.valor); 
    }
  }

  getParameter(id: number){
    this.parameterService.getParametro(id)
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
        
        this.parameterService.setParametro(data)
        .subscribe(response => {
          if (response['success']) {
            Swal.fire('Actualizado', 'parametro actualizado!', 'success')
            this.reloadTable();
          }
          else{
            Swal.fire('Error', '' + response['messages'], 'error')
          }
        }) 
      }
      else{
        Swal.fire('Error', 'Los valores no son correctos', 'error')
      }
      
    }else {
      const data = {
        Id: this.idSend,
        Value: this.valor
      }
      this.parameterService.setParametro(data)
        .subscribe(response => {
          if (response['success']) {
            Swal.fire('Actualizado', 'parametro actualizado!', 'success')
            this.reloadTable();
          }
          else{
            Swal.fire('Error', '' + response['messages'], 'error')
          }
        }) 
    }    
  }  



}
