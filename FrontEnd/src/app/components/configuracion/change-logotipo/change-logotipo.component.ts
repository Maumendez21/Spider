import { Component, OnInit } from '@angular/core';
import { SpiderfleetService } from '../../../services/spiderfleet.service';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from '../../../services/shared.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-change-logotipo',
  templateUrl: './change-logotipo.component.html',
  styleUrls: ['./change-logotipo.component.css']
})
export class ChangeLogotipoComponent implements OnInit {

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService, private shared: SharedService, private router: Router) {

    this.limpiarFiltrosMapa();

    if (this.shared.verifyLoggin()) {




    } else {
      this.router.navigate(['/login']);
    }

  }

  logo = document.getElementById("logotipo");

  limpiarFiltrosMapa() {
    this.shared.limpiarFiltros();
    if (!document.getElementById("sidebarRight").classList.toggle("active")) {
      document.getElementById("sidebarRight").classList.toggle("active")
    }
  }

  tipoArchivo: string;
  tamañoArchivo: number;
  archivo: any;

  ngOnInit(): void {
  }

  changeDocument(file){

    this.tipoArchivo = file.target.files[0].type;
    this.tamañoArchivo = file.target.files[0].size;
    this.archivo = file.target.files[0];

  }

  updateFile(){
    let size = Math.round((this.tamañoArchivo / 1024));

    if (this.tipoArchivo !== "image/png" || size >= 1096) {
      this.toastr.warning("El archivo debe de ser de tipo .png y no debe pesar mas de 1MB");
    } else {

      let formData = new FormData();
      formData.append('File', this.archivo, this.archivo.name);

      this.spiderService.updateLogo(formData)
      .subscribe(response => {
        this.shared.broadcastLogoStream(true);
      });
      this.toastr.success("Recargue la página para ver los cambios.", 'Actualización exitosa');
    }
  }


  uploadFile(){
    let size = Math.round((this.tamañoArchivo / 1024));

    if (this.tipoArchivo !== "image/png" || size >= 1096) {

      this.toastr.warning("El archivo debe de ser de tipo .png y no debe pesar mas de 1MB");

    } else {

      let formData = new FormData();
      formData.append('File', this.archivo, this.archivo.name);
      this.spiderService.setLogo(formData)
      .subscribe(response => {
        this.shared.broadcastLogoStream(true);
      });

      this.toastr.success("Recargue la página para ver los cambios.", 'Carga exitosa');


    }
  }

}
