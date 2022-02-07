import { Component, OnInit } from '@angular/core';
import { ConfigurationService } from '../services/configuration.service';
import { ParametersService } from '../services/parameters.service';

@Component({
  selector: 'app-general-parameters',
  templateUrl: './general-parameters.component.html',
  styleUrls: ['./general-parameters.component.css']
})
export class GeneralParametersComponent implements OnInit {

  parametros: any[] = [];
  id: number;
  valor: number;
  desc: string;

  constructor(
    private parameterService: ParametersService
  ) { 
    this.getParametrosList();
  }

  getId(id: number, valor: number, desc: string){
    this.id = id;
    this.valor = valor;
    this.desc = desc

  }

  getParametrosList = () => {
    this.parameterService.getListParametrosGenerales()
    .subscribe(resp => {
      this.parametros = resp;
    })
  }


  ngOnInit(): void {
  }

}
