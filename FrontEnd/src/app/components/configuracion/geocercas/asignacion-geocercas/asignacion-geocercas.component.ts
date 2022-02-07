import { Component, OnInit } from '@angular/core';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { SharedService } from 'src/app/services/shared.service';

@Component({
  selector: 'app-asignacion-geocercas',
  templateUrl: './asignacion-geocercas.component.html',
  styleUrls: ['./asignacion-geocercas.component.css']
})
export class AsignacionGeocercasComponent implements OnInit {

  selectGeocerca: string = "";
  devicesChecked: any = [];
  geocercas: any;
  dispositivos: any;

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService, private router: Router, private shared: SharedService) {
    if (shared.verifyLoggin()) {
      this.getGeocercasAndDispositivos();
    } else {
      this.router.navigate(['/login']);
    }
  }

  ngOnInit(): void {
  }

  getGeocercasAndDispositivos() {
    this.spiderService.getListGeocercas()
      .subscribe(response => {
        this.geocercas = response;
      });

    this.spiderService.getDevicesGeneralNew("")
      .subscribe(response => {
        this.dispositivos = response["ListLastPosition"];
      });
  }

  getSelectedOptions() {
    return this.dispositivos.filter(opt => opt.checked).map(opt => opt.dispositivo);
  }

  crearAsignacion() {

    if (this.getSelectedOptions().length > 0 && this.selectGeocerca != "") {

      const data = {
        IdGeoFence: this.selectGeocerca,
        ListDevice: this.getSelectedOptions()
      }

      this.spiderService.setNuevaAsignacion(data)
        .subscribe(response => {
          this.router.navigate(['/configuration/geocercas']);
          this.toastr.success('Asignación registrada exitosamente', 'Exito!');
        });
    } else {
      this.toastr.warning('Es necesario seleccionar una geocerca y los dispositivos', 'Faltan campos por seleccionar');
    }
  }

}
