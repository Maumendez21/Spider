import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';

@Component({
  selector: 'app-asignacion-puntos-interes',
  templateUrl: './asignacion-puntos-interes.component.html',
  styleUrls: ['./asignacion-puntos-interes.component.css']
})
export class AsignacionPuntosInteresComponent implements OnInit {

  selectPunto: string = "";
  devicesChecked: any = [];
  puntos: any;
  dispositivos: any;

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService, private router: Router, private shared: SharedService) {
    if (shared.verifyLoggin()) {
      this.getPuntosInteresAndDispositivos();
    } else {
      this.router.navigate(['/login']);
    }
  }

  ngOnInit(): void {
  }

  getPuntosInteresAndDispositivos() {

    this.spiderService.getListPuntosInteres()
      .subscribe(response => {

        this.puntos = response;
      });

    this.spiderService.getDevicesGeneral("Flota", "")
      .subscribe(response => {
        this.dispositivos = response;
      });
  }

  getSelectedOptions() {
    return this.dispositivos.filter(opt => opt.checked).map(opt => opt.dispositivo);
  }

  crearAsignacion() {

    if (this.getSelectedOptions().length > 0 && this.selectPunto != "") {

      const data = {
        IdPointInterest: this.selectPunto,
        ListDevice: this.getSelectedOptions()
      }


      this.spiderService.setVincularPuntoDispositivos(data)
        .subscribe(response => {
          console.log(response);
          if (response['success']) {
            this.router.navigate(['/configuration/puntos-interes']);
            this.toastr.success('Asignación registrada exitosamente', 'Exito!');
          }
        });
    } else {
      this.toastr.warning('Es necesario seleccionar un punto de interes y los dispositivos', 'Faltan campos por seleccionar');
    }
  }

}
