import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';
import { SharedService } from '../../../Services/shared.service';
import { PorfileService } from '../services/porfile.service';

@Component({
  selector: 'app-change-logo',
  templateUrl: './change-logo.component.html',
  styleUrls: ['./change-logo.component.css']
})
export class ChangeLogoComponent implements OnInit {

  constructor(
    private shared: SharedService,
    private porfileService: PorfileService
  ) { }

  ngOnInit(): void {
  }

  logo = document.getElementById("logotipo");

  tipoArchivo: string;
  tamañoArchivo: number;
  archivo: any;

  changeDocument(file){

    this.tipoArchivo = file.target.files[0].type;
    this.tamañoArchivo = file.target.files[0].size;
    this.archivo = file.target.files[0];

  }

  updateFile(){
    let size = Math.round((this.tamañoArchivo / 1024));

    if (this.tipoArchivo !== "image/png" || size >= 1096) {
      Swal.fire('Atención!', 'El archivo debe de ser de tipo .png y no debe pesar mas de 1MB', 'warning')
    } else {
      
      let formData = new FormData();
      formData.append('File', this.archivo, this.archivo.name);
      
      this.porfileService.updateLogo(formData)
      .subscribe(response => {
        this.shared.broadcastLogoStream(true);
      });
      Swal.fire('Actualización exitosa!', 'Recargue la página para ver los cambios.', 'success')
    }
  }
  
  
  uploadFile(){
    let size = Math.round((this.tamañoArchivo / 1024));
    
    if (this.tipoArchivo !== "image/png" || size >= 1096) {

      Swal.fire('Atención!', 'El archivo debe de ser de tipo .png y no debe pesar mas de 1MB', 'warning')
      
    } else {

      let formData = new FormData();
      formData.append('File', this.archivo, this.archivo.name);
      this.porfileService.setLogo(formData)
      .subscribe(response => {
        this.shared.broadcastLogoStream(true);
      });
      
      Swal.fire('Carga exitosa!', 'Recargue la página para ver los cambios.', 'success')
      // this.toastr.success("Recargue la página para ver los cambios.", 'Carga exitosa');


    }
  }

}
