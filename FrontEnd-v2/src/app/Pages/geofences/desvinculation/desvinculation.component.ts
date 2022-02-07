import { Component, OnInit } from '@angular/core';
import { SharedService } from '../../../Services/shared.service';
import { GeofencesService } from '../services/geofences.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-desvinculation',
  templateUrl: './desvinculation.component.html',
  styleUrls: ['./desvinculation.component.css']
})
export class DesvinculationComponent implements OnInit {

  constructor(
    private shared: SharedService,
    private geofencesService: GeofencesService,
    private router: Router
  ) { 
    this.getListDevices();
  }

  nombreGeocerca: string;
  dispositivosGeocerca: any;
  idCollapse: any;

  ngOnInit(): void {
  }

  getListDevices() {
    this.shared.listDevicesStream$.subscribe(data => {
      console.log(data);
      
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
        this.geofencesService.setDeleteAsignacion(data)
          .subscribe(response => {

            Swal.fire({
              title: 'Se desvincularon correctamente los dispositivos',
              confirmButtonText: 'OK',
              icon: 'success'
            }).then((result) => {
              if (result.isConfirmed) {
                this.router.navigate(['/geofences/list']);
              }
            })
          });
      } else {
        Swal.fire({
          icon: 'error',
          title: 'Ningún dispotivo seleccionado'
        })
      }
    }
  }


}
