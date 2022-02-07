import { Component, OnInit, Input } from '@angular/core';
import { ConfigurationService } from 'src/app/Pages/configuration/services/configuration.service';
import { AdminService } from '../../services/admin.service';
import Swal from 'sweetalert2';
import { UsersAdminService } from '../../../configuration/services/users-admin.service';


@Component({
  selector: 'app-action-users-ad',
  templateUrl: './action-users-ad.component.html',
  styleUrls: ['./action-users-ad.component.css']
})
export class ActionUsersAdComponent implements OnInit {

  @Input() reloadTable: any;
  @Input() idSend: number;

  public name: string;
  public lastname: string;
  public email: string;
  public phone: string;
  public title: string;

  constructor(private configurationService: ConfigurationService, private adminService: AdminService, private usersAdminService: UsersAdminService) { 
    
  }

  ngOnInit(): void {
  }

  ngOnChanges(): void {
    if (this.idSend !== 0) {
      this.title = 'Editar'
      this.getUserInfo(this.idSend);
    }else {
      this.title = 'Crear'
      this.cleanValues();
      return;
    }
    
  }

  getUserInfo(id: number) {
    this.usersAdminService.getUsuarioConfiguracion(id)
      .subscribe(data => {
        // console.log(data.Name);
        this.name = data.Name;
        this.lastname = data.LastName;
        this.email = data.Email;
        this.phone = data.Telephone;
      });
  }

  cleanValues(){
    this.name = "";
    this.lastname = "";
    this.email = "";
    this.phone = "";
  }

  actualizarUsuario() {

    if (this.name != "" && this.lastname != "" && this.email != "" && this.phone != "" ) {
      const data = {
        Name: this.name,
        LastName: this.lastname,
        Email: this.email,        
        Telephone: this.phone,   
        UserName : this.idSend
      };
      console.log(data);

      if (this.idSend !== 0) {
        
        this.adminService.setUpdateUser(data)
        .subscribe(response => {
          // console.log(response);
          if (response['success']) {
            this.cleanValues();
            this.reloadTable();
            Swal.fire('Correcto!', 'Exito al Actualizar el usuario', 'success')
           
          } else {
            Swal.fire('Error!', '' + response['messages'], 'error')
          }
        });
      }else {
        Swal.fire('Error!', 'Error al tratar de Actualizar la información', 'error')        
      }
    } else {
      Swal.fire('Warning!', 'Es necesario llenar todos los campos', 'warning')
    }
  }
}