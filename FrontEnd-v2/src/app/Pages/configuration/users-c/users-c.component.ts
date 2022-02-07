import { Component, OnInit } from '@angular/core';
import { ConfigurationService } from '../services/configuration.service';
import { UsersAdminService } from '../services/users-admin.service';

@Component({
  selector: 'app-users-c',
  templateUrl: './users-c.component.html',
  styleUrls: ['./users-c.component.css']
})
export class UsersCComponent implements OnInit {

  public rol = localStorage.getItem('role');

  public id: number;
  public user: string;
  public usuarios: any;
  public pageOfItems: Array<any>;

  public searchValue: string = '';


  constructor(
    private configurationService: ConfigurationService,
    private usersService: UsersAdminService
  ) { 
    this.getListUsuarios(this.searchValue);
  }

  ngOnInit(): void {
  }

  getListUsuarios = (value: string = '') => {
    this.usersService.getListUsuariosConfiguracion(value)
      .subscribe(response => {
        this.usuarios = response.map((x, i) => ({ name: x.Name + " " + x.LastName, email: x.Email, group: x.Grupo, phone: x.Telephone, role: x.DescripcionRol, id: x.UserName }));
      });
  }

  getUsuario(id: number) {
    this.id = id;

  }
  getNombre(user: string){
    this.user = user;


  }

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }



}
