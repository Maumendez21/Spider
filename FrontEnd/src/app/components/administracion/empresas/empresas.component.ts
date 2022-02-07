import { Component, OnInit } from '@angular/core';
import { SpiderfleetService } from '../../../services/spiderfleet.service';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from '../../../services/shared.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-empresas',
  templateUrl: './empresas.component.html',
  styleUrls: ['./empresas.component.css']
})
export class EmpresasComponent implements OnInit {

  empresas: any[] = [];
  pageOfItems: Array<any>;

  habilitado: boolean = false;
  est: number;

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService, private shared: SharedService, private router: Router) {

    this.limpiarFiltrosMapa();

    if (shared.verifyLoggin()) {
      this.getListCompanys();
    } else {
      this.router.navigate(['/login']);
    }
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
    await this.spiderService.setCompanysAccess(data).
    subscribe(response => {
      if (response['success']) {
        if (status) {
          this.toastr.success("Se deshabilito correctamente", "Exito!");
        }else if(!status){
          this.toastr.success("Se habilito correctamente", "Exito!");
        }
      }
      else{
        
        this.toastr.error(response['messages'], "Error!");
      }
    })
    this.habilitado = false;

  }

  limpiarFiltrosMapa() {

    this.shared.limpiarFiltros();

    if (!document.getElementById("sidebarRight").classList.toggle("active")) { 
      document.getElementById("sidebarRight").classList.toggle("active") 
    }
  }

  ngOnInit(): void {
  }

  getListCompanys(){
    this.spiderService.getListCompanies()
    .subscribe(resp => {
      this.empresas = resp;
    })
  }

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }

}
