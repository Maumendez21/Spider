import { Component, OnInit } from '@angular/core';
import { ConfigurationService } from '../../configuration/services/configuration.service';
import { AdminService } from '../services/admin.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-business',
  templateUrl: './business.component.html',
  styleUrls: ['./business.component.css']
})
export class BusinessComponent implements OnInit {

  public rol = localStorage.getItem('role');

  public companias: any;
  public pageOfItems: Array<any>;
  public searchValue: string = '';
  public habilitado: boolean = false;
  public est: number;

  constructor(private adminService: AdminService) {   
    this.getListCompaniasPrincipales();
  }

  ngOnInit(): void {
  }

  getListCompaniasPrincipales = () => {
    this.adminService.getListCompanies()
      .subscribe(response => {
        console.log(response);        
          this.companias = response.map((x, i) => ({ name: x.Name , id: x.Node, status: x.Status, }));
      });
  }

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }

  async cambiar(node: string, status: number){
    if (status) {
      this.est = 0
    }else if(!status){
      this.est = 1
    }
    const data = {
      Node: node,
      Active: this.est
    }
    this.habilitado = true;
    await this.adminService.setCompanysAccess(data).
    subscribe(response => {
      if (response['success']) {
        if (status) {          
          Swal.fire('Correcto!', 'Se deshabilito correctamente', 'success')
        }else if(!status){
          Swal.fire('Correcto!', 'Se habilito correctamente', 'success')
        }
      }
      else{
        Swal.fire('Correcto!', response['messages'], 'error')
      }
    })
    this.habilitado = false;

  }

  

}


