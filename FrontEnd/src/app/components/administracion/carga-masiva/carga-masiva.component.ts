import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from '../../../services/shared.service';
import { ToastrService } from 'ngx-toastr';
import { SpiderfleetService } from '../../../services/spiderfleet.service';

@Component({
  selector: 'app-carga-masiva',
  templateUrl: './carga-masiva.component.html',
  styleUrls: ['./carga-masiva.component.css']
})
export class CargaMasivaComponent implements OnInit {

  companies: any = [];
  compania: string = "0";
  tipoArchivo: string;
  archivo: any;

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService, private shared: SharedService, private router: Router) {

    this.limpiarFiltrosMapa();

    if (this.shared.verifyLoggin()) {
      this.getCompanies();
    } else {
      this.router.navigate(['/login']);
    }

  }

  ngOnInit(): void {
  }

  getCompanies() {
    this.spiderService.getCompanies()
      .subscribe(companies => {
        this.companies = companies;
      });
  }
  pageOfItems: Array<any>;
  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }


  limpiarFiltrosMapa() {
    this.shared.limpiarFiltros();
    if (!document.getElementById("sidebarRight").classList.toggle("active")) {
      document.getElementById("sidebarRight").classList.toggle("active")
    }
  }

  uploadFile(compani: string){
    if (this.tipoArchivo != "text/plain") {
      this.toastr.warning("El archivo debe de ser de tipo .TXT");
    } else {

      let formData = new FormData();

      formData.append('File', this.archivo, this.archivo.name);

      this.spiderService.sendCargaMasiva(compani, formData)
        .subscribe(response => {
          this.toastr.success("Se cargo el archivo correctamente");
        }, err => {
          console.log(err);
        });
    }
  }

  changeDocument(file){
    this.tipoArchivo = file.target.files[0].type;
    this.archivo = file.target.files[0];
  }

}
