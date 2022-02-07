import { Component, OnInit, Input } from '@angular/core';
import Swal from 'sweetalert2';
import { ConfigurationService } from '../../services/configuration.service';

@Component({
  selector: 'app-edit-responsable',
  templateUrl: './edit-responsable.component.html',
  styleUrls: ['./edit-responsable.component.css']
})
export class EditResponsableComponent implements OnInit {
  
  @Input() idSend: number;
  @Input() reloadTable: any;

  id: string;
  nameUpdate: string;
  emailUpdate: string;
  phoneUpdate: string;
  areaUpdate: string;

  action: boolean = false;

  title: string;

  constructor(
    private configurationService: ConfigurationService
  ) { }

  ngOnInit(): void {
  }

  ngOnChanges(): void {
    if (this.idSend !== 0) {
      this.action = true;
      this.getResponsable(this.idSend);
      this.title = 'Editar';
    }else {
      this.cleanValues();
      this.action = false;
      this.title = 'Crear';
      return;
    }

  }

  getResponsable(id: number) {

    this.configurationService.getResponsable(id)
      .subscribe(response => {
        this.id = response.Id;
        this.nameUpdate = response.Name;
        this.emailUpdate = response.Email;
        this.areaUpdate = response.Area;
        this.phoneUpdate = response.Phone;
      });
  }

  cleanValues(){
    this.nameUpdate = "";
    this.emailUpdate = "";
    this.phoneUpdate = "";
    this.areaUpdate = "";
  }

  updateResponsable() {

    const data = {
      Id: this.id,
      Name: this.nameUpdate,
      Email: this.emailUpdate,
      Phone: this.phoneUpdate,
      Area: this.areaUpdate
    };

    if (this.action) {
      
      this.configurationService.setUpdateResponsable(data)
        .subscribe(response => {
  
          if (response['success']) {
            this.reloadTable();
            this.cleanValues();
            Swal.fire('Correcto!', 'Exito al actualizar el responsable', 'success')
          } else {
            Swal.fire('Error!', '' + response['messages'], 'error')
          }
        });
      }else {
        this.configurationService.setNuevoResponsable(data)
        .subscribe(response => {
          if (response['success']) {
            
            Swal.fire('Correcto!', 'Exito al registrar el responsable', 'success')
            this.reloadTable();
            this.cleanValues()
            
          } else {
            Swal.fire('Error!', '' + response['messages'], 'error')
          }
       });
    }


  }


}
