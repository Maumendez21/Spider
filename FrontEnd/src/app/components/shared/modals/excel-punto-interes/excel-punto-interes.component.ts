import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';

@Component({
  selector: 'app-excel-punto-interes',
  templateUrl: './excel-punto-interes.component.html',
  styleUrls: ['./excel-punto-interes.component.css']
})
export class ExcelPuntoInteresComponent implements OnInit {

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService, private shared: SharedService) { }

  tipoArchivo: string;
  archivo: any;

  @Output() refresh: EventEmitter<boolean> = new EventEmitter()

  ngOnInit(): void {
  }

  changeDocument(file){

    console.log(file);

    this.tipoArchivo = file.target.files[0].type;
    this.archivo = file.target.files[0];
  }

  uploadFile(){
    if (this.tipoArchivo != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
      this.toastr.warning("El archivo debe de ser de tipo .xlsx");
    } else {

      let formData = new FormData();
      formData.append('File', this.archivo, this.archivo.name);

      this.spiderService.postImportXSLX(formData)
        .subscribe(response => {
          this.refresh.emit(true);
          this.toastr.success("Se cargo el archivo correctamente");
        }, err => {
          console.log(err);
        });
    }
  }
}
