import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';

@Component({
  selector: 'app-eliminar-asignacion-puntos-interes',
  templateUrl: './eliminar-asignacion-puntos-interes.component.html',
  styleUrls: ['./eliminar-asignacion-puntos-interes.component.css']
})
export class EliminarAsignacionPuntosInteresComponent implements OnInit {

  name: string;
  devices: any;
  idCollapse: any;

  constructor(private shared: SharedService, private spiderService: SpiderfleetService, private toastr: ToastrService, private router: Router) {
    if (shared.verifyLoggin()) {
      this.getListDevices();
    } else {
      this.router.navigate(['/login']);
    }
  }

  ngOnInit(): void {
  }

  

  getListDevices() {
    this.shared.listDevicesStream$.subscribe(data => {
      this.name = data['name'];
      this.devices = data['listDevices'];
    });
  }

  getSelectedOptions() {
    return this.devices.filter(opt => opt.checked).map(opt => opt.Id);
  }

  desvincular() {

    if (this.devices != null) {

      const devices = this.getSelectedOptions();

      if (devices.length > 0) {
        const data = {
          ListDeviceId: this.getSelectedOptions()
        }
        this.spiderService.setDesvincularDispositivosPunto(data)
          .subscribe(response => {
            this.toastr.success('Se desvincularon correctamente los dispositivos', 'Exito!');
            this.router.navigate(['/configuration/puntos-interes']);
          });
      } else {
        this.toastr.warning('Es necesario seleccionar que dispositivos se desvincularan', 'Ningun dispositivo seleccionado');
      }
    }
  }

}
