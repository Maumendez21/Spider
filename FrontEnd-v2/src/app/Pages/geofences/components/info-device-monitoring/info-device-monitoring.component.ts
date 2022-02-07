import { Component, Input, OnInit } from '@angular/core';
import { InfoDevice } from '../../../home/models/info-device';

@Component({
  selector: 'app-info-device-monitoring',
  templateUrl: './info-device-monitoring.component.html',
  styleUrls: ['./info-device-monitoring.component.css']
})
export class InfoDeviceMonitoringComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  @Input() infoDevice: InfoDevice;



  getEstatusName(estatus: number) {
    switch(estatus) {
      case 1:
        return "Activo";
      case 2:
        return "Inactivo";
      case 3:
        return "Inactivo/Warning";
      case 4:
        return "Falla/Error";
      case 5:
        return "Activo sin movimiento";
      case 6:
        return "Paro";
      case 7:
        return "Pánico";
    }
  }

}
