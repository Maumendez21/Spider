import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';
import { ConfigurationService } from '../../services/configuration.service';
import { InfoVehicleService } from '../../services/info-vehicle.service';

@Component({
  selector: 'app-add-version',
  templateUrl: './add-version.component.html',
  styleUrls: ['./add-version.component.css']
})
export class AddVersionComponent implements OnInit {

  constructor(private infoVehicle: InfoVehicleService) { }
  version: string;

  ngOnInit(): void {
  }

  agregarVersion(){

    if (this.version !== null) {

      const data = {
        Description: this.version
      };

      this.infoVehicle.setNewVersion(data)
      .subscribe(response => {
        if (response['success']) {
          Swal.fire('Agregada!', 'Versión agregada correctamente', 'success')
        }else{
          Swal.fire('ERROR!', '' + response['messages'], 'error')
        }
        
      });
      
      
    } else {
      Swal.fire('Cuidado!', 'Es necesario llenar todos los campos', 'warning')
    }

  }

}
