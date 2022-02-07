import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';
import { ConfigurationService } from '../services/configuration.service';

@Component({
  selector: 'app-responsables',
  templateUrl: './responsables.component.html',
  styleUrls: ['./responsables.component.css']
})
export class ResponsablesComponent implements OnInit {

  responsables: any;
  pageOfItems: Array<any>;
  public searchValue: string='';
  id: number;


  constructor(private configurationService: ConfigurationService) { 
    this.getListResponsables(this.searchValue);
  }

  ngOnInit(): void {
  }

  getListResponsables = (responsible: string = '') => {
    this.configurationService.getListResponsables(responsible)
      .subscribe(response => {
        this.responsables = response.map((x, i) => ({ area: x.Area, email: x.Email, id: x.Id, name: x.Name, phone: x.Phone }));
      });
  }

  getResponsable(id: number) {
    this.id = id;
  }

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }

  deleteResponsible(id: number, name: string) {
    Swal.fire({
      title: `¿Estas seguro que quieres eliminar a ${name}?`,
      text: "Esta acción ya no se puede revertir",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Si, lo entiendo'
    }).then((result) => {
      if (result.value) {

        this.configurationService.deleteResponsable(id)
          .subscribe(response => {
            if (response['success']) {
              this.getListResponsables();
              Swal.fire('Correcto!', 'Responsable eliminado correctamente', 'success')
            } else {
              Swal.fire('Error!', 'No se pudo eliminar el Responsable, intentalo nuevamente', 'error')
            }
          });
      }
    })
  }



}
