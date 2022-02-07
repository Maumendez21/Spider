import { Component, OnInit } from '@angular/core';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-rutas',
  templateUrl: './rutas.component.html',
  styleUrls: ['./rutas.component.css']
})
export class RutasComponent implements OnInit {

  rutas: any = [];

  constructor(private spiderService: SpiderfleetService) {
    this.getRoutes();
  }

  ngOnInit(): void {
  }

  getRoutes() {
    this.spiderService.getListRutasConfiguracion()
      .subscribe(rutas => {

        this.rutas = rutas;
      });
  }

  refresh(event: any){
    if (event) {
      this.getRoutes();
    }
  }

  eliminarRuta(id: string, index: number) {

    Swal.fire({
      title: '¿Seguro que quieres eliminar la ruta?',
      text: "¡Esta acción no se puede revertir!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Si, estoy seguro',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {

        this.spiderService.deleteRutaConfiguracion(id)
          .subscribe(response => {

            if (response['success']) {

              this.rutas.splice(index, 1);

              Swal.fire(
                '¡Eliminado!',
                'La ruta fue eliminada con exito.',
                'success'
              )
            } else {
              Swal.fire(
                '¡Error!',
                'La ruta no pudo ser eliminada, intentalo nuevamente.',
                'error'
              )
            }
          });
      }
    })
  }

}
