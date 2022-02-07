import { Component, OnInit } from '@angular/core';
import { ConfigurationService } from '../../configuration/services/configuration.service';
import { UsersAdminService } from '../../configuration/services/users-admin.service';

@Component({
  selector: 'app-users-a',
  templateUrl: './users-a.component.html',
  styleUrls: ['./users-a.component.css']
})
export class UsersAComponent implements OnInit {

  public rol = localStorage.getItem('role');

  public id: number;
  public user: string;
  public usuarios: any;
  public pageOfItems: Array<any>;
  public searchValue: string = '';

  constructor(private configurationService: ConfigurationService, private userService: UsersAdminService) { 
    this.getListUsuarios(this.searchValue);
  }

  ngOnInit(): void {
  }

  getListUsuarios = (value: string = '') => {
    this.userService.getListUsuariosConfiguracion(value)
      .subscribe(response => {
      this.usuarios = response.map((x, i) => ({ name: x.Name + " " + x.LastName, email: x.Email, 
          group: x.Grupo, phone: x.Telephone, role: x.DescripcionRol, 
          id: x.UserName, }));
      });
  }

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }

  getUsuario(id: number) {
    this.id = id;
  }
  getNombre(user: string){
    this.user = user;
  }

}
