import { Component, OnInit } from '@angular/core';
import { MobilityService } from '../services/mobility.service';
import { SharedService } from '../../../Services/shared.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
declare const google: any;


@Component({
  selector: 'app-poinst-interest',
  templateUrl: './poinst-interest.component.html',
  styleUrls: ['./poinst-interest.component.css']
})
export class PoinstInterestComponent implements OnInit {

  puntos: any = [];
  pageOfItems: Array<any>;

  loading: boolean = true;

  constructor(
    private mobilityService: MobilityService,
    private shared: SharedService,
    private router: Router
  ){
    this.preparePermisos();
  }

  permisos: string;

  preparePermisos() {
    // this.shared.permisosStream$.subscribe(response => {
    //   this.permisos = response;

    // });

    if (!this.permisos) {
      this.permisos = localStorage.getItem('permits');
    }
  }
  


  ngOnInit(): void {
    this.getListPuntoInteres();
  }

  refresh(event: any){
    if (event) {
      this.getListPuntoInteres();
    }
  }

  async getListPuntoInteres() {
    this.loading = true;
    await this.mobilityService.getListPuntosInteresDispositivos()
    .subscribe(response => {
      
      this.puntos = response.map((x, i) => ({ Name: x.Name, Description: x.Description, Id: x.Id, ListDevice: x.ListDevice }));
      this.loading = false;
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
    this.router.navigate(['/mobility/point-desvinculation']);
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

        this.mobilityService.deletePuntoInteres(id)
        .subscribe(response => {
          if (response['success']) {
            // this.toastr.success("Punto de interes eliminado exitosamente!", "Exito!");
            this.getListPuntoInteres();
          } else {
            // this.toastr.warning('No se pudo actualizar el Punto de Interes, intentalo nuevamente', 'Al parecer algo ocurrio');
          }
        });
      }
    })
  }
  






}
