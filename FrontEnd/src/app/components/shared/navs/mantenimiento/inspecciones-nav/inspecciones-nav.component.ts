import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { data } from 'jquery';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-inspecciones-nav',
  templateUrl: './inspecciones-nav.component.html',
  styleUrls: ['./inspecciones-nav.component.css']
})
export class InspeccionesNavComponent implements OnInit {

  public inspecciones: any = [];

  constructor(private spiderService: SpiderfleetService, private router: Router, private shared: SharedService, private toastr: ToastrService) {




    if (shared.verifyLoggin()) {

      this.getInspecciones();

      this.limpiarFiltrosMapa();
    } else {
      this.router.navigate(['/login']);
    }
  }


  getInspecciones(){
    this.spiderService.getInspeccionList().subscribe(data => {
      this.inspecciones = data;
    })
  }



  showInspect(folio: string){
    this.spiderService.getInspecctionFolio(folio).subscribe(data => {

    })
  }

  limpiarFiltrosMapa() {

    this.shared.limpiarFiltros();

    if (!document.getElementById("sidebarRight").classList.toggle("active")) {
      document.getElementById("sidebarRight").classList.toggle("active")
    }
  }




  ngOnInit(): void {
  }

}
