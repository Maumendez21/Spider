import { Component, OnInit, Input } from '@angular/core';
import Swal from 'sweetalert2';
import { ConfigurationService } from '../../services/configuration.service';
import { UsersAdminService } from '../../services/users-admin.service';

@Component({
  selector: 'app-action-users',
  templateUrl: './action-users.component.html',
  styleUrls: ['./action-users.component.css']
})
export class ActionUsersComponent implements OnInit {

  @Input() reloadTable: any;
  @Input() idSend: number;



  public rolesList: any;
  public estatusList: any;
  public gruposList: any;

  public name: string;
  public lastname: string;
  public email: string;
  public username: string;
  public phone: string;
  public group: string;
  public rol: string;
  public estatus: string;
  public password: string;
  public title: string;

  constructor(
    private configurationService: ConfigurationService,
    private usersService: UsersAdminService
  ) { 
    this.getRoles();
    this.getEstatus();
    this.getGrupos();
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
    this.usersService.getUsuarioConfiguracion(id)
      .subscribe(data => {
        // console.log(data.Name);
        this.name = data.Name;
        this.lastname = data.LastName;
        this.email = data.Email;
        this.username = data.UserName;
        this.phone = data.Telephone;
        this.group = data.Hierarchy;
        this.rol = data.IdRole;
        this.estatus = data.IdStatus;
      });
  }

  getRoles() {
    this.usersService.getRoles()
      .subscribe(data => {
        // console.log(data);
        this.rolesList = data;
      });
  }

  getEstatus() {
    this.usersService.getEstatus()
      .subscribe(data => {
        // console.log(data);
        this.estatusList = data;
      });
  }

  getGrupos() {
    this.configurationService.getListSubempresas()
      .subscribe(data => {
        // console.log(data);
        this.gruposList = data;
      });
  }

  cleanValues(){
    this.name = "";
    this.lastname = "";
    this.email = "";
    this.username = "";
    this.password = "";
    this.phone = "";
    this.group = "";
    this.rol = "";
    this.estatus = "";
  }

  actualizarUsuario() {

    if (this.name != "" && this.lastname != "" && this.email != "" && this.username != "" && this.phone != "" && this.group != "" && this.rol != "" && this.estatus != "") {
      const data = {
        Name: this.name,
        LastName: this.lastname,
        Email: this.email,
        UserName: this.username,
        Telephone: this.phone,
        Grupo: this.group,
        Role: this.rol,
        Status: this.estatus
      };

      if (this.idSend !== 0) {
        
        this.usersService.setUpdateUsuarioConfiguracion(data)
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
        this.usersService.setNewUsuarioConfiguracion(data)
        .subscribe(response => {
          if (response['success']) {

            this.reloadTable();
            Swal.fire('Correcto!', 'Exito al registrar el usuario', 'success')

           this.cleanValues();

          } else {
            Swal.fire('Error!', '' + response['messages'], 'error')
          }
        });
      }
    } else {
      Swal.fire('Warning!', 'Es necesario llenar todos los campos', 'warning')
    }
    
  }





  

}
