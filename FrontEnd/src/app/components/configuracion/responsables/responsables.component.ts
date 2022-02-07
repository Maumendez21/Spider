import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import Swal from 'sweetalert2';

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

  constructor(private spiderService: SpiderfleetService, private shared: SharedService, private router: Router, private toastr: ToastrService) {
    if (this.shared.verifyLoggin()) {
      this.getListResponsables(this.searchValue);
    } else {
      this.router.navigate(['/login']);
    }
  }

  ngOnInit(): void {}

  getListResponsables = (responsible: string = '') => {
    this.spiderService.getListResponsables(responsible)
      .subscribe(response => {
        this.responsables = response.map((x, i) => ({ area: x.Area, email: x.Email, id: x.Id, name: x.Name, phone: x.Phone }));
      });
  }

  getResponsable(id: number) {
    this.id = id;
  }

  deleteResponsible(id: number, name: string) {
    Swal.fire({
      title: `¿Estas seguro que quieres eliminar a ${name}?`,
      text: "Esta acción ya no se puede revertir",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#ff6e40',
      cancelButtonColor: '#555555',
      confirmButtonText: 'Si, lo entiendo'
    }).then((result) => {
      if (result.value) {

        this.spiderService.deleteResponsable(id)
          .subscribe(response => {
            if (response['success']) {
              this.toastr.success(`${name} eliminado exitosamente!`, "Exito!");
              this.getListResponsables();
            } else {
              this.toastr.warning('No se pudo eliminar el Responsable, intentalo nuevamente', 'Al parecer algo ocurrio');
            }
          });
      }
    })
  }

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }

}
