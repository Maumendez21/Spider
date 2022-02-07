import { Component, OnInit, Input } from '@angular/core';
import Swal from 'sweetalert2';
import { permission } from '../../models/permission';
import { ConfigurationService } from '../../services/configuration.service';
import { UsersAdminService } from '../../services/users-admin.service';

@Component({
  selector: 'app-permissions-users',
  templateUrl: './permissions-users.component.html',
  styleUrls: ['./permissions-users.component.css']
})
export class PermissionsUsersComponent implements OnInit {

  @Input() user: string;
  @Input() reloadTable: any;

  permisos: any = [];
  responsePermisos = new Array<permission>();

  constructor(
    private configurationService: ConfigurationService,
    private usersService: UsersAdminService
  ) { }

  ngOnInit(): void {
  }
  
  ngOnChanges(): void {
    //Called before any other lifecycle hook. Use it to inject dependencies, but avoid any serious work here.
    //Add '${implements OnChanges}' to the class.

    console.log(this.user);
    
    this.listPermisos(this.user);
    
  }

  listPermisos(user: string){
    console.log(user);
    
    this.usersService.getPermissions(user)
    .subscribe(response => {
      console.log(response);
      
      this.permisos = response;
    })
  }

  generarArreglo(modulo: string, estatus: boolean){
    const data: permission = {
      IdUser: this.user,
      Modulo: modulo,
      Active: estatus
    }
    this.responsePermisos.push(data);
    // console.log(this.responsePermisos);
  }

  asignarPermisos(){
    let cadena: string;
    let bandera: boolean = false;
    // Se recorre el arreglo
    this.responsePermisos.some(function(item){
      //Se asigna el valor en una variable
      cadena = item.Modulo;
      // Valida si existe un elemento con la propiedad CON
      if (cadena.indexOf('CON') != -1) {
        bandera = true;
        return true;
      }
    })

    if (bandera) {
      const data: permission = {
        IdUser: this.user,
        Modulo: 'CON',
        Active: true
      }
      this.responsePermisos.push(data);
    }

    const data = {
      PermissionList: this.responsePermisos
    };

    this.usersService.setPermission(data)
      .subscribe(response => {
        if (response['success']) {
          Swal.fire('Correcto!', 'Exito al asignar permisos.', 'success')
          this.responsePermisos = [];
          this.reloadTable();
        } else {
          Swal.fire('ERROR!', '' + response['messages'], 'error')
        }
      });
  }

}
