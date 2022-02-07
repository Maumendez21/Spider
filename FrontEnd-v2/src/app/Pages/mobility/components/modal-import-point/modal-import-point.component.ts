import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import Swal from 'sweetalert2';
import { MobilityService } from '../../services/mobility.service';

@Component({
  selector: 'app-modal-import-point',
  templateUrl: './modal-import-point.component.html',
  styleUrls: ['./modal-import-point.component.css']
})
export class ModalImportPointComponent implements OnInit {

  
  tipoArchivo: string;
  archivo: any;

  @Output() refresh: EventEmitter<boolean> = new EventEmitter()
  constructor(
    private mobilityService: MobilityService
  ) { }

  ngOnInit(): void {
  }

  changeDocument(file){

    console.log(file);

    this.tipoArchivo = file.target.files[0].type;
    this.archivo = file.target.files[0];
  }

  uploadFile(){
    if (this.tipoArchivo != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
      Swal.fire('Warning', 'El  archivo tiene que ser de tipo .xlsx', 'warning')
    } else {
      
      let formData = new FormData();
      formData.append('File', this.archivo, this.archivo.name);
      
      this.mobilityService.postImportXSLX(formData)
      .subscribe(response => {
        this.refresh.emit(true);
        Swal.fire('Echo', 'Se cargo el archivo correctamente', 'success')
      }, err => {
        console.log(err);
      });
    }
  }

}
