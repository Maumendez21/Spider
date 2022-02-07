import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';

@Component({
  selector: 'app-kml-rutas',
  templateUrl: './kml-rutas.component.html',
  styleUrls: ['./kml-rutas.component.css']
})
export class KmlRutasComponent implements OnInit {

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService, private shared: SharedService) { }

  tipoArchivo: string;
  archivo: any;

  @Output() refresh: EventEmitter<boolean> = new EventEmitter()

  ngOnInit(): void {
  }

  changeDocument(file){

    console.log(file);

    this.tipoArchivo = file.target.files[0].name;
    this.archivo = file.target.files[0];
  }

  uploadFile(){
    if (this.tipoArchivo.indexOf('.kml') === -1) {
      this.toastr.warning("El archivo debe de ser de tipo .kml");
    } else {
      let formData = new FormData();
      formData.append('File', this.archivo, this.archivo.name);
      this.spiderService.postImportKml(formData)
        .subscribe(response => {
          console.log(response["success"]);
          if (response["success"]) {
            this.refresh.emit(true);
            this.toastr.success("Se cargo el archivo correctamente");
          }else {
            this.toastr.error("Error al cargar el archivo.");
          }
        }, err => {
          this.toastr.error("Error: " + err);
        });
    }
  }

}
