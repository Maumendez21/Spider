import { Component, OnInit } from '@angular/core';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { SharedService } from 'src/app/services/shared.service';

@Component({
  selector: 'app-geocercas',
  templateUrl: './geocercas.component.html',
  styleUrls: ['./geocercas.component.css']
})
export class GeocercasComponent implements OnInit {

  geocercas: any;
  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService, private router: Router, private shared: SharedService) {
    this.limpiarFiltrosMapa();

    if (shared.verifyLoggin()) {
      this.getAllGeocercas();
    } else {
      this.router.navigate(['/login']);
    }
  }

  ngOnInit(): void {
  }

  limpiarFiltrosMapa() {

    this.shared.limpiarFiltros();

    if (!document.getElementById("sidebarRight").classList.toggle("active")) {
      document.getElementById("sidebarRight").classList.toggle("active")
    }
  }

  getAllGeocercas() {
    this.spiderService.getGeocercasAsignacion()
      .subscribe(data => {
        this.geocercas = data;
      });
  }

  desvincularGeocerca(listDevices: any, name: string) {

    const nameAndListDevices = {
      name: name,
      listDevices: listDevices
    }

    this.shared.broadcastListDevicesStream(nameAndListDevices);
    this.router.navigate(['/configuration/desvincular-asignacion']);
  }

  actualizarGeocerca(id: number) {
    this.router.navigate(['configuration/actualizar-geocerca/', id]);
  }

  eliminarGeocerca(id: number) {
      Swal.fire({
        title: '¿Estas seguro que quieres eliminar la geocerca?',
        text: "Esta acción ya no se puede revertir",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Si, lo entiendo'
      }).then((result) => {
        if (result.value) {
          this.spiderService.setDeleteGeocerca(id.toString())
            .subscribe(data => {
              this.toastr.success("Geocerca eliminada exitosamente!", "Exito!");
              this.getAllGeocercas();
            });
        }
      })
  }

}
