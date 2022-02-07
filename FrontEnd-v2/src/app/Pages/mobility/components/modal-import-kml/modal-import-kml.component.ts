import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import Swal from 'sweetalert2';
import { MobilityService } from '../../services/mobility.service';

@Component({
  selector: 'app-modal-import-kml',
  templateUrl: './modal-import-kml.component.html',
  styleUrls: ['./modal-import-kml.component.css']
})
export class ModalImportKMLComponent implements OnInit {

  tipoArchivo: string;
  archivo: any;
  @Output() refresh: EventEmitter<boolean> = new EventEmitter()


  constructor(
    private mobilityService: MobilityService
  ) { }

  ngOnInit(): void {
  }

  changeDocument(file){
    this.tipoArchivo = file.target.files[0].name;
    this.archivo = file.target.files[0];
  }

  uploadFile(){
    if (this.tipoArchivo.indexOf('.kml') === -1) {
      Swal.fire({
        icon: 'error',
        title: 'El archivo debe de ser de tipo .kml'
      })
    } else {
      let formData = new FormData();
      formData.append('File', this.archivo, this.archivo.name);
      this.mobilityService.postImportKml(formData)
        .subscribe(response => {
          console.log(response["success"]);
          if (response["success"]) {
            this.refresh.emit(true);
            Swal.fire({
              icon: 'success',
              title: 'Se cargo el archivo correctamente'
            })
            
          }else {
            Swal.fire({
              icon: 'error',
              title: 'Error al cargar el archivo.'
            })
          }
        }, err => {
          Swal.fire({
            icon: 'error',
            title: 'Error: ' + err
          })
        });
    }
  }

}
