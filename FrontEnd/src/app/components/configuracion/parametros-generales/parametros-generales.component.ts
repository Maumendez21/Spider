import { Component, OnInit } from '@angular/core';
import { SpiderfleetService } from '../../../services/spiderfleet.service';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from '../../../services/shared.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-parametros-generales',
  templateUrl: './parametros-generales.component.html',
  styleUrls: ['./parametros-generales.component.css']
})
export class ParametrosGeneralesComponent implements OnInit {

  parametros: any[] = [];
  id: number;
  valor: number;
  desc: string;

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService, private shared: SharedService, private router: Router) {

    this.limpiarFiltrosMapa();

    if (this.shared.verifyLoggin()) {
      this.getParametrosList();
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

  getId(id: number, valor: number, desc: string){
    this.id = id;
    this.valor = valor;
    this.desc = desc

  }

  getParametrosList = () => {
    this.spiderService.getListParametrosGenerales()
    .subscribe(resp => {
      this.parametros = resp;
    })
  }

  ngOnInit(): void {
  }

}
