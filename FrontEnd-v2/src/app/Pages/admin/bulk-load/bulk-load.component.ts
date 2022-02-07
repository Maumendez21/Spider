import { Component, OnInit } from '@angular/core';
import { ConfigurationService } from '../../configuration/services/configuration.service';
import { AdminService } from '../services/admin.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-bulk-load',
  templateUrl: './bulk-load.component.html',
  styleUrls: ['./bulk-load.component.css']
})
export class BulkLoadComponent implements OnInit {

  public rol = localStorage.getItem('role');

  public companias: any;
  public pageOfItems: Array<any>;
  
  public habilitado: boolean = false;
  public est: number;

  public tipoArchivo: string;
  public archivo: any;

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.getListCompanias();
  }

  getListCompanias = () => {
    this.adminService.getCompanies()
      .subscribe(response => {
          this.companias = response.map((x, i) => ({ name: x.Name, id: x.Hierarchy }));
      });
  }

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }

  uploadFile(compani: string){
    if (this.tipoArchivo != "text/plain") {      
      Swal.fire('Información!', 'El archivo debe de ser de tipo .TXT', 'info')
    } else {

      let formData = new FormData();

      formData.append('File', this.archivo, this.archivo.name);

      this.adminService.sendCargaMasiva(compani, formData)
        .subscribe(response => {
          Swal.fire('Correcto!', 'Se cargo el archivo correctamente', 'info')
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
