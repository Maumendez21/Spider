import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-puntos-interes',
  templateUrl: './puntos-interes.component.html',
  styleUrls: ['./puntos-interes.component.css']
})
export class PuntosInteresComponent implements OnInit {

  puntos: any = [];
  pageOfItems: Array<any>;

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService, private router: Router, private shared: SharedService) { }

  ngOnInit(): void {
    this.getListPuntoInteres();
  }

  refresh(event: any){
    if (event) {
      this.getListPuntoInteres();
    }
  }

  getListPuntoInteres() {
    this.spiderService.getListPuntosInteresDispositivos()
      .subscribe(response => {

        this.puntos = response.map((x, i) => ({ Name: x.Name, Description: x.Description, Id: x.Id, ListDevice: x.ListDevice }));
      });
  }

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }

  desvincularRuta(listDevices: any, name: string) {

    const nameAndListDevices = {
      name: name,
      listDevices: listDevices
    }

    this.shared.broadcastListDevicesStream(nameAndListDevices);
    this.router.navigate(['configuration/puntos-interes/desvincular-asignacion']);
  }

  eliminarPuntoInteres(id: string, index: number) {

    Swal.fire({
      title: '¿Estas seguro que quieres eliminar el punto de interes?',
      text: "Esta acción ya no se puede revertir",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Si, lo entiendo'
    }).then((result) => {
      if (result.value) {

        this.spiderService.deletePuntoInteres(id)
          .subscribe(response => {
            if (response['success']) {
              this.toastr.success("Punto de interes eliminado exitosamente!", "Exito!");
              this.getListPuntoInteres();
            } else {
              this.toastr.warning('No se pudo actualizar el Punto de Interes, intentalo nuevamente', 'Al parecer algo ocurrio');
            }
          });
      }
    })
  }

}
