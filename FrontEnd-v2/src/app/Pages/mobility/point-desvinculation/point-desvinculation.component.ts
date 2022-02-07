import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';
import { SharedService } from '../../../Services/shared.service';
import { Router } from '@angular/router';
import { MobilityService } from '../services/mobility.service';

@Component({
  selector: 'app-point-desvinculation',
  templateUrl: './point-desvinculation.component.html',
  styleUrls: ['./point-desvinculation.component.css']
})
export class PointDesvinculationComponent implements OnInit {

  name: string;
  devices: any;
  idCollapse: any;

  constructor(
    private shared: SharedService,
    private router: Router,
    private mobilityService: MobilityService
  ) { 
    this.getListDevices();
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
        this.mobilityService.setDesvincularDispositivosPunto(data)
          .subscribe(response => {
            


            Swal.fire({
              title: 'Se desvincularon correctamente los dispositivos',
              confirmButtonText: 'OK',
              icon: 'success'
            }).then((result) => {
              if (result.isConfirmed) {
                this.router.navigate(['/mobility/points-interest']);
              }
            })
          });
      } else {
        Swal.fire('Error', 'Es necesario seleccionar que dispositivos se desvincularan', 'error');
      }
    }
  }

}
