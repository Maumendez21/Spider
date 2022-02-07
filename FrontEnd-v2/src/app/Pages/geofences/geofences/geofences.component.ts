import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GeofencesService } from '../services/geofences.service';
import { SharedService } from '../../../Services/shared.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-geofences',
  templateUrl: './geofences.component.html',
  styleUrls: ['./geofences.component.css']
})
export class GeofencesComponent implements OnInit {

  constructor(
    private geofencesService: GeofencesService,
    private router: Router,
    private shared: SharedService
  ) {
    this.getAllGeocercas();
  }

  
  public loading: boolean = true;
  public geocercas: any;
  public pageOfItems:Array<any>;

  ngOnInit(): void {
  }

  async getAllGeocercas() {
    await this.geofencesService.getGeocercasAsignacion()
      .subscribe(data => {

        this.geocercas = data;
        this.loading = false;
      });
  }

  desvincularGeocerca(listDevices: any, name: string) {

    const nameAndListDevices = {
      name: name,
      listDevices: listDevices
    }

    this.shared.broadcastListDevicesStream(nameAndListDevices);
    this.router.navigate(['/geofences/desvinculation']);
  }

  actualizarGeocerca(id: number) {
    this.router.navigate(['/geofences/geofence', id]);
  }

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }

  eliminarGeocerca(id: number) {
    Swal.fire({
      title: '¿Estas seguro que quieres eliminar la geocerca?',
      text: "Esta acción ya no se puede revertir",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Si, lo entiendo'
    }).then((result) => {
      if (result.value) {
        this.geofencesService.setDeleteGeocerca(id.toString())
          .subscribe(data => {
            Swal.fire({
              position: 'top-end',
              icon: 'success',
              title: 'Geocerca eliminada',
              showConfirmButton: false,
              timer: 1500
            })
            this.getAllGeocercas();
          });
      }
    })
}


}
