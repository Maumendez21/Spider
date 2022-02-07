import { Component, OnInit } from '@angular/core';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-eliminar-asignacion-geocerca',
  templateUrl: './eliminar-asignacion-geocerca.component.html',
  styleUrls: ['./eliminar-asignacion-geocerca.component.css']
})
export class EliminarAsignacionGeocercaComponent implements OnInit {

  nombreGeocerca: string;
  dispositivosGeocerca: any;
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
      this.nombreGeocerca = data['name'];
      this.dispositivosGeocerca = data['listDevices'];
    });
  }

  getSelectedOptions() {
    return this.dispositivosGeocerca.filter(opt => opt.checked).map(opt => opt.Id);
  }

  desvincular() {

    if (this.dispositivosGeocerca != null) {
      const devices = this.getSelectedOptions();

      if (devices.length > 0) {
        const data = {
          ListDeviceId: this.getSelectedOptions()
        }
        this.spiderService.setDeleteAsignacion(data)
          .subscribe(response => {
            this.toastr.success('Se desvincularon correctamente los dispositivos', 'Exito!');
            this.router.navigate(['/configuration/geocercas']);
          });
      } else {
        this.toastr.warning('Es necesario seleccionar que dispositivos se desvincularan', 'Ningun dispositivo seleccionado');
      }
    }
  }

}
